

using Microsoft.EntityFrameworkCore;
using sms_server.Entities;

public class SMSDbContext : DbContext
{

    public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserCodeEntity> UserCode => Set<UserCodeEntity>();

    //Don't forget to put the model builder below _ ;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCodeEntity>()
            .HasOne<UserEntity>()               // Reference to UserEntity
            .WithMany()                         // No navigation on UserEntity
            .HasForeignKey(uc => uc.UserId);
        base.OnModelCreating(modelBuilder);
    }
}
