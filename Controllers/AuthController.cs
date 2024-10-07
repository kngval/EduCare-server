
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using sms_server.Dtos;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthInterface authService;
    public AuthController(IAuthInterface authService)
    {
        this.authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpDto authDto)
    {
        try
        {

            if (string.IsNullOrWhiteSpace(authDto.email) || string.IsNullOrWhiteSpace(authDto.email))
            {
                return BadRequest("Invalid Email");
            }
            if (string.IsNullOrWhiteSpace(authDto.password) || string.IsNullOrWhiteSpace(authDto.password))
            {
                return BadRequest("Invalid Password");
            }
            if (string.IsNullOrWhiteSpace(authDto.role) || string.IsNullOrWhiteSpace(authDto.role))
            {
                return BadRequest("Role is required");
            }

           var res = await authService.SignUp(authDto);
            if(res == null)
            {
              return Conflict(res);
            }
            return Ok(res);
        } catch(Exception ex)
        {
          Console.WriteLine(ex.Message);
          return StatusCode(500,"An unexpected error occured.");
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      if(string.IsNullOrEmpty(loginDto.email) || string.IsNullOrWhiteSpace(loginDto.email))
      {
        return BadRequest("Email is required");
      }
      if(string.IsNullOrEmpty(loginDto.password) || string.IsNullOrWhiteSpace(loginDto.password))
      {
        return BadRequest("Email is required");
      }
      
      var token = await authService.Login(loginDto);

      return Ok(token);
      
    }
}
