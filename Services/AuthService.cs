using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//
using educare_server.Mapping;
using sms_server.Dtos;
using sms_server.Entities;
using sms_server.Response;

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

            if (string.IsNullOrEmpty(SignUpDto.email) || string.IsNullOrWhiteSpace(SignUpDto.email))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Email field is required",
                    Field = "email"
                };
            }
            if (string.IsNullOrEmpty(SignUpDto.password) || string.IsNullOrWhiteSpace(SignUpDto.password))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Password field is required",
                    Field = "password"
                };
            }
            if (string.IsNullOrEmpty(SignUpDto.role) || string.IsNullOrWhiteSpace(SignUpDto.role))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Please select a role",
                    Field = "role"
                };
            }


            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == SignUpDto.email);
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

    //admin signup service
    public async Task<AuthResponse> SignUpAdmin(AdminDto adminDto)
    {
        if (string.IsNullOrEmpty(adminDto.code))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Code is required for teacher/admin access",
                Field = "code"
            };
        }
        if (string.IsNullOrEmpty(adminDto.email) || string.IsNullOrWhiteSpace(adminDto.email))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Email field is required",
                Field = "email"
            };
        }
        if (string.IsNullOrEmpty(adminDto.password) || string.IsNullOrWhiteSpace(adminDto.password))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Password field is required",
                Field = "password"
            };
        }
        if (string.IsNullOrEmpty(adminDto.role) || string.IsNullOrWhiteSpace(adminDto.role))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Please select a role",
                Field = "role"
            };
        }

        var userCode = await context.UserCode.FirstOrDefaultAsync(c => c.Code == adminDto.code);
        if (userCode == null || userCode.Available == false)
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Invalid Code",
                Field = "code"
            };
        }
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == adminDto.email);

        if (user != null)
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Email already exists",
                Field = "email"
            };
        }
        else
        {

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(adminDto.password);
            var userEntity = new UserEntity()
            {
                Email = adminDto.email,
                Password = passwordHash,
                Role = adminDto.role
            };

            await context.Users.AddAsync(userEntity);
            await context.SaveChangesAsync();
            userCode.UserId = userEntity.Id;
            userCode.Available = false;

            await context.SaveChangesAsync();

            return new AuthResponse()
            {
                Success = true,
                Message = "User created successfully !"
            };
        }


    }

    //Login service
    public async Task<AuthResponse> Login(LoginDto loginDto)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.email);
            if (string.IsNullOrEmpty(loginDto.email) || string.IsNullOrWhiteSpace(loginDto.email))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Email field is required",
                    Field = "email"
                };
            }
            if (string.IsNullOrEmpty(loginDto.password) || string.IsNullOrWhiteSpace(loginDto.password))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Password field is required",
                    Field = "password"
                };
            }

            if (string.IsNullOrEmpty(loginDto.role) || string.IsNullOrWhiteSpace(loginDto.role))
            {
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Role field is required",
                    Field = "role"
                };
            }

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

                if (user.Role != "student")
                {
                    return new AuthResponse()
                    {
                        Success = false,
                        Message = "User's registered role is not a valid student",
                        Field = "role"
                    };
                }
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

    public async Task<AuthResponse> LoginAdmin(AdminDto adminDto)
    {
        if (string.IsNullOrEmpty(adminDto.code))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Code is required for teacher/admin access",
                Field = "code"
            };
        }
        if (string.IsNullOrEmpty(adminDto.email) || string.IsNullOrWhiteSpace(adminDto.email))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Email field is required",
                Field = "email"
            };
        }
        if (string.IsNullOrEmpty(adminDto.password) || string.IsNullOrWhiteSpace(adminDto.password))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Password field is required",
                Field = "password"
            };
        }
        if (string.IsNullOrEmpty(adminDto.role) || string.IsNullOrWhiteSpace(adminDto.role))
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Please select a role",
                Field = "role"
            };
        }

        var userCode = await context.UserCode.FirstOrDefaultAsync(c => c.Code == adminDto.code);
        if (userCode == null || userCode.Available == true)
        {
            return new AuthResponse()
            {
                Success = false,
                Message = "Invalid Code",
                Field = "code"
            };
        }
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == adminDto.email);
        if (user == null)
        {

            return new AuthResponse()
            {
                Success = false,
                Message = "User does not exist",
                Field = "email"
            };
        }

        if (userCode.UserId != user.Id)
        {

            return new AuthResponse()
            {
                Success = false,
                Message = "Code already used",
                Field = "code"
            };
        }

        var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(adminDto.password, user.Password);
        if (!isPasswordCorrect)
        {

            return new AuthResponse()
            {
                Success = false,
                Message = "Incorrect password.",
                Field = "password"
            };
        }

        var token = GenerateToken(user.Id, user.Email, user.Role);

        return new AuthResponse()
        {
            Success = true,
            Message = "Login Successful !",
            Token = token
        };

    }

    private string GenerateToken(int id, string email, string role)
    {
        List<Claim> claims = new List<Claim>(){
        new Claim(JwtRegisteredClaimNames.Sub,id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email,email),
        new Claim(ClaimTypes.Role,role)
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
