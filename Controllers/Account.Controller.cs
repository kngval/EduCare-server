

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/account")]
public class AccountController:ControllerBase {
  private readonly IUserInfoInterface userInfoService; 
  public AccountController(IUserInfoInterface userInfoService)
  {
    this.userInfoService = userInfoService;
  }
  
  [HttpGet("/get-userinfo")]
  public Task<IActionResult> GetUserInfo(int id)
  {
    var user = userInfoService.FetchUser(id);
  }
}
