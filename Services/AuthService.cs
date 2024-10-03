

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sms_server.Dtos;
using sms_server.Entities;

public class AuthService:IAuthInterface {
  private readonly SMSDbContext context;
  private readonly IConfiguration config;
  public AuthService(SMSDbContext context, IConfiguration config)
  {
    this.context = context;
    this.config = config;
  }

    public async Task<UserEntity?> SignUp(AuthDto authDto)
    {
      var user = await context.Users.FirstOrDefaultAsync(u => u.Email == authDto.email);
      if(user != null)
      {
        return null;
      } else {

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(authDto.password);

        UserEntity newUser = new UserEntity(){
          Email = authDto.email,
          Password = passwordHash,
          Role = authDto.role
        };
        
        await context.Users.AddAsync(newUser); 
        await context.SaveChangesAsync();
        return newUser;
      }

    }

    // private string GenerateToken(int id, string username)
    // {
    //   var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value!)); 
    //   
    //
    // }

    // DATABASE ACCESS GOES HERE 
}
