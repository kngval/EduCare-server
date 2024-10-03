

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


using sms_server.Mapping;
using sms_server.Dtos;
using sms_server.Entities;

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
    public async Task<AuthResponse> SignUp(AuthDto authDto)
    {
        // check first if user exists
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == authDto.email);
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
                 BCrypt.Net.BCrypt.HashPassword(authDto.password);

                // UserEntity newUser = new UserEntity()
                // {
                //     Email = authDto.email,
                //     Password = authDto.password,
                //     Role = authDto.role
                // };

                await context.Users.AddAsync(authDto.toAuthEntity());
                await context.SaveChangesAsync();
                return new AuthResponse()
                {
                  Success = true,
                  Message = "User created successfully",
                  User = authDto.toAuthEntity()
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


    // private string GenerateToken(int id, string username)
    // {
    //   var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value!)); 
    //   
    //
    // }

}
