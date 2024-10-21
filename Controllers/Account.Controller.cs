

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IUserInfoInterface userInfoService;
    public AccountController(IUserInfoInterface userInfoService)
    {
        this.userInfoService = userInfoService;
    }
    [HttpGet("test")]
    public string Test()
    {
        return "Endpoint hit!";
    }

    [HttpGet("get-userinfo")]
    public IActionResult GetUserInfo()
    {
        var userIdClaimValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if(userIdClaimValue == null)
        {
          return BadRequest("Invalid token");
        }
        int userId = int.Parse(userIdClaimValue);
        var user = userInfoService.FetchUser(userId);
        return Ok(user);
    }
}
