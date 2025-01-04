

public interface IGradesInterface {
  GradingResponse GradeStudent(GradesRequest gradesRequest); 
  List<RoomToStudentEntity> FetchStudentGrades(int studentId);  
}
