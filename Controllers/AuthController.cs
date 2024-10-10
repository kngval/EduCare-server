
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
           var res = await authService.SignUp(authDto);
            if(res.Success == false)
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
    [HttpPost("signup/admin")]
    public async Task<IActionResult> SignUpAdmin(AdminDto adminDto)
    {
      if(adminDto.role != "admin" && adminDto.role != "teacher")
      {
        return BadRequest("Not allowed !");
      }
      try{
        var response = await authService.SignUpAdmin(adminDto); 
        if(response.Success == false)
        {
          return BadRequest(response);
        }
        return Ok(response);
      }catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        return StatusCode(500,"An unexpected error occured.");
      }

    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      
      try{
      var response = await authService.Login(loginDto);
      if(response.Success == false)
      {
        return BadRequest(response);
      } 
      return Ok(response);
      } catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        return StatusCode(500,"An unexpected error occured.");
      }
      
    }
    [HttpPost("login/admin")]
    public async Task<IActionResult> LoginAdmin(AdminDto adminDto)
    {
      if(adminDto.role != "admin" && adminDto.role != "teacher")
      {
        return BadRequest("Not allowed !");
      }
      try{
        var response = await authService.LoginAdmin(adminDto);
        if(response.Success == false)
        {
          return BadRequest(response);
        }

        return Ok(response);

      } catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        return StatusCode(500,"An unexpected error occured.");
      }
    }
}
