
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize(Policy = "admin")]
[ApiController]
[Route("api/admin")]
public class AdminController:ControllerBase {
  
  [HttpGet("test")]
  public IActionResult AdminEndpoint(){
    return Ok("Hi User");
  }
  
  [HttpPost("create-code")]
  public IActionResult CreateCode(){
      
  }

  private string GenerateRandomCode(int codeLength){
    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; 
    StringBuilder randomStrBuilder = new StringBuilder(codeLength);
    Random random = new Random();
    for(int i = 0; i < codeLength; i++){
      int randomIndex = random.Next(alphabet.Length); 
      char randomChar = alphabet[randomIndex];

      randomStrBuilder.Append(randomChar);
    }
    return randomStrBuilder.ToString();
  }
}
