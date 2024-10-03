

using Microsoft.EntityFrameworkCore;
using sms_server.Entities;

public class SMSDbContext:DbContext {

  public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options){}

  public DbSet<UserEntity> Users => Set<UserEntity>(); 
  public DbSet<StudentEntity> Student => Set<StudentEntity>(); 
  public DbSet<AdminEntity> Admin => Set<AdminEntity>(); 
  public DbSet<TeacherEntity> Teacher => Set<TeacherEntity>(); 

  //Don't forget to put the model builder below _ ;
}
