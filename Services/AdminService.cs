
using System.Text;

public class AdminService:IAdminService {
    public Task GenerateCode(int length)
    {
        throw new NotImplementedException();
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
