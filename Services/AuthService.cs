

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService:AuthInterface {
  private readonly DbContext _context;
  public AuthService(DbContext context)
  {
    _context = context;
  }

  private string GenerateToken(int id, string username)
  {
    //create the JWT Settings later for the secret key paguwi mo hashsahahaha
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkeyheredontforget")); 

  }

    // DATABASE ACCESS GOES HERE 
}
