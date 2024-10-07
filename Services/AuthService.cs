

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


using sms_server.Mapping;
using sms_server.Dtos;
using sms_server.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public class AuthService : IAuthInterface
{
    private readonly SMSDbContext context;
    private readonly IConfiguration config;
    private readonly ILogger<AuthService> logger;
    public AuthService(SMSDbContext context, IConfiguration config, ILogger<AuthService> logger)
    {
        this.context = context;
        this.config = config;
        this.logger = logger;
    }

    //Signup Method
    public async Task<AuthResponse> SignUp(SignUpDto SignUpDto)
    {
        // check first if user exists
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == SignUpDto.email);

            if (string.IsNullOrWhiteSpace(SignUpDto.email) || string.IsNullOrWhiteSpace(SignUpDto.email))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Email field is required",
                    Field = "email"
                };
            }
            if (string.IsNullOrWhiteSpace(SignUpDto.password) || string.IsNullOrWhiteSpace(SignUpDto.password))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Password field is required",
                    Field = "password"
                };
            }
            if (string.IsNullOrWhiteSpace(SignUpDto.role) || string.IsNullOrWhiteSpace(SignUpDto.role))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Password field is required"
                };
            }
            if (user != null)
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = $"Email {SignUpDto.email} already exists",
                    Field = "email"
                };
            }
            else
            {
                //create the user in the database
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(SignUpDto.password);
                var userEntity = SignUpDto.toAuthEntity();
                userEntity.Password = hashedPassword;
                await context.Users.AddAsync(userEntity);
                await context.SaveChangesAsync();
                return new AuthResponse()
                {
                    Success = true,
                    Message = $"User created successfully",
                    User = SignUpDto.toAuthEntity()
                };
            }
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError($"Error SigningUp User '{SignUpDto.email}' : {dbEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
    public async Task<AuthResponse> Login(LoginDto loginDto)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.email);
            if (user == null)
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "User does not exist",
                    Field = "email"
                };
            }
            else
            {
                var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginDto.password, user.Password);
                if (!isPasswordCorrect)
                {
                    return new AuthResponse()
                    {
                        Success = false,
                        Message = "Wrong password",
                        Field = "password"
                    };
                }

                var token = GenerateToken(user.Id, user.Email, user.Role);
                return new AuthResponse()
                {
                    Success = true,
                    Message = "Login success!",
                    Token = token
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error logging in");
            throw;
        }

    }


    private string GenerateToken(int id, string email, string role)
    {
        List<Claim> claims = new List<Claim>(){
        new Claim(JwtRegisteredClaimNames.Sub,id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email,email),
      };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Issuer = config.GetSection("Jwt:Issuer").Value,
            Audience = config.GetSection("Jwt:Audience").Value
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);

    }

}
