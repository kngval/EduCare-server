
using System.Text;
using sms_server.Entities;
using sms_server.Response;


public class AdminService : IAdminService
{
    private readonly SMSDbContext context;

    public AdminService(SMSDbContext context)
    {
        this.context = context;
    }



    // CODE SERVICES 
    public List<UserCodeWithUserDTO> GetAllCodes()
    {
        var result = from userCode in context.UserCode
                     join user in context.Users
                     on userCode.UserId equals user.Id
                     into userGroup
                     from user in userGroup.DefaultIfEmpty()
                     select new UserCodeWithUserDTO
                     {
                         UserCode = userCode,
                         UserEmail = user != null ? user.Email : null
                     };
        return result.ToList<UserCodeWithUserDTO>();
        // return context.UserCode.ToList<UserCodeEntity>();
    }

    public AdminResponse DeleteCode(int codeId)
    {
        var code = context.UserCode.SingleOrDefault(c => c.Id == codeId);
        if (code == null)
        {
            return new AdminResponse()
            {
                Success = false,
                Message = "Code id not found"
            };

        }
        context.UserCode.Remove(code);
        context.SaveChanges();
        return new AdminResponse(){
          Success = true,
          Message = "Code deleted successfully !"
        };
    }

    public AdminResponse GenerateCode(int length)
    {
        if (length <= 0)
        {
            return new AdminResponse()
            {
                Success = false,
                Message = "Code length not valid."
            };
        }
        var code = Helpers.GenerateRandomCode(length);

        var dbCode = context.UserCode.FirstOrDefault(c => c.Code == code);

        if (dbCode != null)
        {
            return new AdminResponse()
            {
                Success = false,
                Message = "Code already exists, generate a new one"
            };
        }
        var newCode = new UserCodeEntity()
        {
            Code = code,
            Available = true,
        };
        context.UserCode.Add(newCode);
        context.SaveChanges();

        return new AdminResponse()
        {
            Success = true,
            Message = "Code created successfully !"
        };
    }

}
