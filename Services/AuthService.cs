

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService:AuthInterface {
  private readonly DbContext _context;
  private readonly IConfiguration _config;
  public AuthService(DbContext context, IConfiguration config)
  {
    _context = context;
    _config = config;
  }

  private string GenerateToken(int id, string username)
  {
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!)); 


  }

    // DATABASE ACCESS GOES HERE 
}
