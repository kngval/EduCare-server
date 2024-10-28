

using Microsoft.EntityFrameworkCore;
using sms_server.Entities;

public class SMSDbContext : DbContext
{

    public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserCodeEntity> UserCode => Set<UserCodeEntity>();
    public DbSet<UserInfoEntity> UserInfo => Set<UserInfoEntity>();
    public DbSet<RoomEntity> Rooms => Set<RoomEntity>();

    //Don't forget to put the model builder below _ ;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCodeEntity>()
            .HasOne<UserEntity>()               // Reference to UserEntity
            .WithMany()                         // No navigation on UserEntity
            .HasForeignKey(uc => uc.UserId);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserInfoEntity>()
          .HasOne<UserEntity>()
          .WithMany()
          .HasForeignKey(ui => ui.UserId);
      
        //CREATE THE MODEL BUILDER FOR ROOM ENTITY
    }
}
