using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PackAndPromote.Database;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurações do JWT - Define a chave secreta para o token JWT, que pode ser obtida das variáveis de ambiente ou do arquivo de configuração.
var jwtSecretKey = Environment.GetEnvironmentVariable("Jwt_SecretKey")
    ?? builder.Configuration["Jwt_SecretKey"];

// Verifica se a chave secreta do JWT está definida; se não estiver, lança uma exceção.
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("A chave secreta JWT não está definida.");
}

// Converte a chave secreta para um array de bytes para ser utilizada na configuração do token.
var key = Encoding.ASCII.GetBytes(jwtSecretKey);

// Configura o serviço de autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema padrão para autenticação
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // Define o esquema padrão para desafios de autenticação
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Desativa o requisito de HTTPS para obter tokens (útil para desenvolvimento)
    options.SaveToken = true;             // Salva o token JWT no contexto da autenticação
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,           // Desabilita a validação do emissor do token
        ValidateAudience = false,         // Desabilita a validação da audiência do token
        ValidateLifetime = true,          // Habilita a validação do tempo de vida do token
        ValidateIssuerSigningKey = true,  // Habilita a validação da chave de assinatura do emissor
        IssuerSigningKey = new SymmetricSecurityKey(key), // Define a chave de assinatura usada para validar o token
        ClockSkew = TimeSpan.Zero         // Remove a margem de erro padrão para o tempo de expiração do token
    };
});

// Adiciona serviços ao contêiner da aplicação
builder.Services.AddControllers(); // Adiciona suporte para controllers

builder.Services.AddEndpointsApiExplorer(); // Adiciona o serviço para geração automática de endpoints para a documentação

// Configuração do Swagger para gerar a documentação da API
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Pack and Promote WebAPI", Version = "v1.0" }); });

// Configura a conexão com o banco de dados usando a string de conexão obtida das variáveis de ambiente ou do arquivo de configuração
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_PackAndPromote")
    ?? builder.Configuration.GetConnectionString("PackAndPromote");

builder.Services.AddDbContext<DbPackAndPromote>(options =>
    options.UseSqlServer(connectionString)); // Configura o DbContext para usar o SQL Server

// Configuração do CORS (Cross-Origin Resource Sharing) para permitir requisições de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Permite requisições de qualquer origem
              .AllowAnyMethod()   // Permite qualquer método HTTP (GET, POST, etc.)
              .AllowAnyHeader();  // Permite qualquer cabeçalho
    });
});

var app = builder.Build();

// Ativando o CORS usando a política definida anteriormente
app.UseCors("AllowAll");

// Configura o pipeline de requisições HTTP
app.UseSwagger(); // Habilita a geração da documentação Swagger
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API V1.0")); // Define o endpoint para a interface do Swagger

app.UseAuthentication(); // Habilita o middleware de autenticação para validar tokens JWT
app.UseAuthorization();  // Habilita o middleware de autorização

app.MapControllers(); // Mapeia os controladores para os endpoints definidos

app.Run(); // Executa a aplicação