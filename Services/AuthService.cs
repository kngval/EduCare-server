

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
    public AuthService(SMSDbContext context, IConfiguration config)
    {
        this.context = context;
        this.config = config;
    }

    //Signup Method
    public async Task<AuthResponse> SignUp(SignUpDto SignUpDto)
    {
        // check first if user exists
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == SignUpDto.email);
            if (user != null)
            {
                return new AuthResponse()
                {
                  Success = false,
                  Message = "User already exists",
                  User = null
                };
            }
            else
            {
                //create the user in the database
                 BCrypt.Net.BCrypt.HashPassword(SignUpDto.password);

                // UserEntity newUser = new UserEntity()
                // {
                //     Email = SignUpDto.email,
                //     Password = SignUpDto.password,
                //     Role = SignUpDto.role
                // };

                await context.Users.AddAsync(SignUpDto.toAuthEntity());
                await context.SaveChangesAsync();
                return new AuthResponse()
                {
                  Success = true,
                  Message = "User created successfully",
                  User = SignUpDto.toAuthEntity()
                };
            }
        }
        catch(DbUpdateException dbEx){
          Console.WriteLine(dbEx.Message);
          throw;
        }
        catch (Exception ex)
        {
         Console.WriteLine(ex.Message);
         throw;
        }

    }
    public async Task<string> Login(LoginDto loginDto)
    {
      var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.email);
      if(user == null)
      {
        return "User does not exist."; 
      }

    }


    private string GenerateToken(int id, string username)
    {
      List<Claim> claims = new List<Claim>(){
        new Claim(JwtRegisteredClaimNames.Email)
      };
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value!)); 
      

    }

}
