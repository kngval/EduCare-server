
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; using System.Security.Claims;
//------------------
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//JWT AUTHENTICATION
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
});
//SERVICES (REPO)
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IUserInfoInterface, UserInfoService>();
//DB Context
builder.Services.AddDbContext<SMSDbContext>(opt => {
      
    var dbConnection = Environment.GetEnvironmentVariable("SMS_CONNECTION");
    if(string.IsNullOrEmpty(dbConnection))
    {
      throw new InvalidOperationException("Invalid Database Connection string");
    }
    opt.UseNpgsql(dbConnection);

    });
builder.Services.AddCors(opt => {
    opt.AddPolicy("AllowSpecificOrigin", policy => {
         policy.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod();
        });
    });



//CONTROLLERS
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();

