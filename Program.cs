using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//------------------
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

//JWT AUTHENTICATION
builder
    .Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ValidAudience = builder.Configuration["Jwt:Audience"],
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = (context) =>
            {
                if (
                    context.Request.Path.StartsWithSegments("/api/auth/login")
                    || context.Request.Path.StartsWithSegments("/api/auth/signup")
                )
                {
                    return Task.CompletedTask;
                }

                var token = context
                    .Request.Headers["Authorization"]
                    .FirstOrDefault()
                    ?.Split(" ")
                    .Last();

                if (string.IsNullOrEmpty(token))
                {
                    context.NoResult();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(
                        "{\"error\": \"Authorization token is missing.\"}"
                    );
                }
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                var message = context.Exception.Message;
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var jsonMessage =
                    context.Exception is SecurityTokenExpiredException
                        ? "Token has expired."
                        : "Invalid token.";

                return context.Response.WriteAsync(
                    $"{{\"error\": \"{jsonMessage}\", \"details\": \"{message}\"}}"
                );
            },

            OnChallenge = context =>
            {
                context.HandleResponse();
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(
                        "{\"error\": \"You are not authorized to access this resource.\"}"
                    );
                }
                return Task.CompletedTask;
            },
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
builder.Services.AddScoped<IRoomService, RoomService>();

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
    opt.AddPolicy(
        "AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers();

//swagger ui
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter 'Bearer' followed by a space with your jwt token ex : Bearer ey2sadjawldawdw...",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
