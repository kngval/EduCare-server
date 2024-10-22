

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sms_server.Dtos;
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
    [HttpGet("get-userinfo")]
    public IActionResult GetUserInfo()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return BadRequest("User Id not present in the token.");
        }
        var user = userInfoService.FetchUser(userId.Value);
        if(user == null)
        {
          return NotFound($"User with the Id of '{userId.Value}' not found");
        }
        return Ok(user);
    }
    [HttpPost("create-userinfo")]
    public async Task<IActionResult> CreateUserInfo(UserInfoDto userInfoDto)
    {
      var userId = GetUserId(); 
      if (userId == null)
      {
        return BadRequest("User Id not present in the token.");
      }
      var res = await userInfoService.CreateUserInfo(userInfoDto,userId.Value);
      if(res.Success == false){
        return BadRequest(res);
      }
      return Ok(res);
    }
    
    //Method for getting the UserId from the auth token
    private int? GetUserId()
    {
        var userIdClaimValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaimValue == null)
        {
            return null;
        }
        return int.Parse(userIdClaimValue);
    }
}
