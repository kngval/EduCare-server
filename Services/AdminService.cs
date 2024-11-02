
using System.Text;
using sms_server.Entities;
using sms_server.Response;

public class AdminService:IAdminService {
    private readonly SMSDbContext context;
    
    public AdminService(SMSDbContext context){
      this.context = context;
    }

    public List<UserCodeEntity> GetAllCodes(){
      return context.UserCode.ToList<UserCodeEntity>();
    } 

    public AdminResponse GenerateCode(int length)
    {
      if(length <= 0){
        return new AdminResponse(){
          Success = false,
          Message = "Code length not valid."
        };
      }
      var code = GenerateRandomCode(length);
      
      var dbCode = context.UserCode.FirstOrDefault(c => c.Code == code);
      
      if(dbCode != null){
        return new AdminResponse(){
          Success = false ,
          Message = "Code already exists, generate a new one"
        };
      }
      var newCode = new UserCodeEntity(){
        Code = code,
        Available = true
      };
      context.UserCode.AddAsync(newCode);
      context.SaveChangesAsync();

      return new AdminResponse(){
        Success = true,
        Message = "Code created successfully !"
      };
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
