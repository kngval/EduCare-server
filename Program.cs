using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
//------------------
var builder = WebApplication.CreateBuilder(args);
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
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = (context) =>

        {
            if (context.Request.Path.StartsWithSegments("/api/auth/login") ||
                           context.Request.Path.StartsWithSegments("/api/auth/signup"))
            {
                return Task.CompletedTask;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.NoResult();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\": \"Authorization token is missing.\"}");
            }
            return Task.CompletedTask;
        },


        OnAuthenticationFailed = context =>
        {
            var message = context.Exception.Message; 
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var jsonMessage = context.Exception is SecurityTokenExpiredException
                ? "Token has expired."
                : "Invalid token.";


            return context.Response.WriteAsync($"{{\"error\": \"{jsonMessage}\", \"details\": \"{message}\"}}");
        },

        OnChallenge = context =>
        {
            context.HandleResponse(); if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json"; return context.Response.WriteAsync("{\"error\": \"You are not authorized to access this resource.\"}");
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
});
//SERVICES (REPO)
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IUserInfoInterface, UserInfoService>();
builder.Services.AddScoped<IAdminService, AdminService>();
//DB Context
builder.Services.AddDbContext<SMSDbContext>(opt =>
{
    var dbConnection = Environment.GetEnvironmentVariable("Educare_DB"); //DATABASE CONNECTION
    if (string.IsNullOrEmpty(dbConnection))
    {
        throw new InvalidOperationException("Invalid Database Connection string");
    }
    opt.UseNpgsql(dbConnection);

});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
//CONTROLLERS
builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

