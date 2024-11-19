

using sms_server.Entities;
using sms_server.Response;

public interface IAdminService {
  AdminResponse GenerateCode(int length);  
  List<UserCodeWithUserDTO> GetAllCodes(); 
  AdminResponse DeleteCode(int codeId);

}
