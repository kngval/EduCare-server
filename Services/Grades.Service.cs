


using Microsoft.EntityFrameworkCore;

public class GradesService : IGradesInterface
{
    private readonly SMSDbContext context;

    public GradesService(SMSDbContext context)
    {
        this.context = context;
    }
    public List<RoomToStudentEntity> FetchStudentGrades(int studentId)
    {
        var grades = context.RoomsToStudent.Where(r => r.StudentId == studentId).Include(r => r.Room);
        return grades.ToList();
    }

    public GradingResponse GradeStudent(GradesRequest gradesRequest)
    {
        if (gradesRequest == null || gradesRequest.studentId <= 0 || gradesRequest.roomId <= 0)
        {
            return new GradingResponse
            {
                Success = false,
                Message = "Invalid request data"
            };
        }
        var studentRooms = context.RoomsToStudent.Where(r => r.StudentId == gradesRequest.studentId);
        if (studentRooms == null)
        {
            return new GradingResponse
            {
                Success = false,
                Message = "Student does not exist"
            };
        }
        var studentGrade = studentRooms.FirstOrDefault(r => r.RoomId == gradesRequest.roomId);
        if (studentGrade == null)
        {
            return new GradingResponse
            {
                Success = false,
                Message = "Room does not exist"
            };
        }

        studentGrade.Grade = gradesRequest.grade;
        context.SaveChanges();

        return new GradingResponse
        {
            Success = true,
            Message = "Student graded successfully"
        };
    }
}
