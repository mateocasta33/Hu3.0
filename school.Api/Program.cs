using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using school.Application.Interfaces;
using school.Application.Services;
using school.Domain.Interfaces;
using school.Infrastructure.Data;
using school.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// DATABASE
// ----------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=localhost;Port=3306;Database=SchoolDataBase;User=root;Password=1234";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 43))));

// ----------------------------
// DEPENDENCY INJECTION
// ----------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

// ----------------------------
// JWT
// ----------------------------
var jwtKey = builder.Configuration["Jwt:Key"] ?? "LlavePorDefectoSuperSegura1234567890!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SchoolApiDefault";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SchoolApiUsersDefault";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ----------------------------
// CORS
// ----------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ----------------------------
// Controllers + Swagger
// ----------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------------
// Middleware
// ----------------------------
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Endpoint raíz para probar
app.MapGet("/", () => Results.Ok(new { message = "API corriendo correctamente" }));

// Swagger en producción
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API V1");
    c.RoutePrefix = "swagger"; // acceder con /swagger
});

app.MapControllers();

app.Run();
