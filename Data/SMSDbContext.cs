

using Microsoft.EntityFrameworkCore;
using sms_server.Entities;

public class SMSDbContext:DbContext {

  public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options){}

  public DbSet<UserEntity> User => Set<UserEntity>(); 
  public DbSet<StudentEntity> Student => Set<StudentEntity>(); 
  public DbSet<AdminEntity> Admin => Set<AdminEntity>(); 
  public DbSet<TeacherEntity> Teacher => Set<TeacherEntity>(); 


}
