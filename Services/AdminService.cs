
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

    //Room Services
    public CreateRoomResponse CreateRoom(RoomDto roomDto)
    {
        if (string.IsNullOrEmpty(roomDto.RoomName) || string.IsNullOrWhiteSpace(roomDto.RoomName))
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "Room Name is required"
            };
        }
        if (string.IsNullOrWhiteSpace(roomDto.teacherEmail) || string.IsNullOrEmpty(roomDto.teacherEmail))
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "Teacher email is required"
            };
        }
        var user = context.Users.Find(roomDto.teacherEmail);
        if (user == null)
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "User not found"
            };
        }

        if (user.Role != "teacher")
        {
            return new CreateRoomResponse()
            {
                Success = false,
                Message = "User is not a valid teacher"
            };
        }

        var room = context.Rooms.Add(new RoomEntity()
        {
            SubjectName = roomDto.RoomName,
            TeacherId = user.Id
        });
        context.SaveChangesAsync();
        return new CreateRoomResponse()
        {
            Success = true,
            Message = "Room created successfully !"
        };
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
        var code = GenerateRandomCode(length);

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

    private string GenerateRandomCode(int codeLength)
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder randomStrBuilder = new StringBuilder(codeLength);
        Random random = new Random();
        for (int i = 0; i < codeLength; i++)
        {
            int randomIndex = random.Next(alphabet.Length);
            char randomChar = alphabet[randomIndex];

            randomStrBuilder.Append(randomChar);
        }
        return randomStrBuilder.ToString();
    }
}
