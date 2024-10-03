
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using sms_server.Dtos;

[ApiController]
[Route("api/auth")]
public class AuthController:ControllerBase
{
    private readonly IAuthInterface authService;
    public AuthController(IAuthInterface authService)
    {
      this.authService = authService;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<string>> SignUp(AuthDto authDto)
    {
      if(string.IsNullOrWhiteSpace(authDto.email))
      {
        return BadRequest("Invalid Email");
      }
      if(string.IsNullOrWhiteSpace(authDto.password))
      {
        return BadRequest("Invalid Password");
      }

      var res = await authService.SignUp(authDto); 

      return Ok(res);
    }
}
