using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PackAndPromote.Database;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurações do JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]; // Chave JWT
var key = Encoding.ASCII.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Pack and Promote WebAPI DEV", Version = "v1.0" }); });

builder.Services.AddDbContext<DbPackAndPromote>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PackAndPromote")));

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API V1.0"));

app.UseAuthentication();
app.UseAuthorization();

// Ativando o CORS
app.UseCors("AllowAll");

app.MapControllers();

app.Run();