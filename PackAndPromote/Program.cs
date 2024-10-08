using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PackAndPromote.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Pack and Promote WebAPI", Version = "v1.0" }); });

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API V1.0"));
}

// Desabilitando o Redirect HTTPS
// app.UseHttpsRedirection();

app.UseAuthorization();

// Ativando o CORS
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
