

using Microsoft.EntityFrameworkCore;

public class AuthService:AuthInterface {
  private readonly DbContext _context;
  public AuthService(DbContext context)
  {
    _context = context;
  }

  // DATABASE ACCESS GOES HERE 
}
