
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
    public async Task<IActionResult> SignUp(AuthDto authDto)
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
}
