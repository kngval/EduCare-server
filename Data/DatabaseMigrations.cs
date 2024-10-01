
using Microsoft.EntityFrameworkCore;

public static class DatabaseMigrations {
  public static void MigrateDb(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SMSDbContext>();
    dbContext.Database.Migrate();
  }
}
