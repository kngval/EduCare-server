

using sms_server.Response;

public interface IAdminService {
  AdminResponse GenerateCode(int length);  
}
