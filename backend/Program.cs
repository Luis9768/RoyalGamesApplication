using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RoyalGamess.Aplications.Autenticacao;
using RoyalGamess.Aplications.Services;
using RoyalGamess.Contexts;
using RoyalGamess.Interfaces;
using RoyalGamess.Repositorys;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Value: Bearer TokenJWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuração de conexão PostgreSQL (com suporte a Default e DATABASE_URL)
string? rawConnStr = builder.Configuration.GetConnectionString("Default") 
                     ?? Environment.GetEnvironmentVariable("DATABASE_URL")
                     ?? builder.Configuration["DATABASE_URL"];

string connectionString = ParsePostgresConnectionString(rawConnStr);

builder.Services.AddDbContext<Royal_GamesssContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Usurio
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
//AutenticaoJwt
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<GeradorTokenJWT>();
// Genero
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<GeneroService>();

// Classifica��o Indicativa
builder.Services.AddScoped<IClassificacaoIndicativa, ClassificacaoIndicativaRepository>();
builder.Services.AddScoped<ClassificacaoService>();

builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<JogoService>();

builder.Services.AddScoped<ILogAlteracaoJogoRepository, LogAlteracaoJogoRepository>();
builder.Services.AddScoped<LogAlteracaoJogoService>();

//promocao
builder.Services.AddScoped<IPromocaoRepository, PromocaoRepository>();
builder.Services.AddScoped<PromocaoService>();

//plataforma
builder.Services.AddScoped<IPlataformaRepository, PlataformaRepository>();
builder.Services.AddScoped<PlataformaService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         // L� a chave secreta definida no appsettings.json.
         // Essa chave � usada para ASSINAR o token quando ele � gerado
         // e tamb�m para VALIDAR se o token recebido � verdadeiro.
         var chave = builder.Configuration["Jwt:Key"]!;

         // Quem emitiu o token (ex: nome da sua aplica��o).
         // Serve para evitar aceitar tokens de outro sistema.
         var issuer = builder.Configuration["Jwt:Issuer"]!;

         // Para quem o token foi criado (normalmente o frontend ou a pr�pria API).
         // Tamb�m ajuda a garantir que o token pertence ao seu sistema.
         var audience = builder.Configuration["Jwt:Audience"]!;

         // Define as regras que ser�o usadas para validar o token recebido.
         options.TokenValidationParameters = new TokenValidationParameters
         {
             // Verifica se o emissor do token � v�lido
             // (se bate com o issuer configurado).
             ValidateIssuer = true,

             // Verifica se o destinat�rio do token � v�lido
             // (se bate com o audience configurado).
             ValidateAudience = true,

             // Verifica se o token ainda est� dentro do prazo de validade.
             // Se j� expirou, a requisi��o ser� negada.
             ValidateLifetime = true,

             // Verifica se a assinatura do token � v�lida.
             // Isso garante que o token n�o foi alterado.
             ValidateIssuerSigningKey = true,

             // Define qual emissor � considerado v�lido.
             ValidIssuer = issuer,

             // Define qual audience � considerado v�lido.
             ValidAudience = audience,

             // Define qual chave ser� usada para validar a assinatura do token.
             // A mesma chave usada na gera��o do JWT deve estar aqui.
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(chave)
             )
         };
     });



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();      

// Inicialização automática do schema e seed do banco PostgreSQL
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<Royal_GamesssContext>();
        RoyalGamess.Aplications.Data.DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao inicializar/conectar com o PostgreSQL.");
    }
}

// Swagger habilitado para testes locais e em deploy
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Royal Games API v1");
    c.RoutePrefix = "swagger";
});

// Ordem correta de middlewares no ASP.NET Core
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Endpoint de Health Check para Easypanel / Docker
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.MapControllers();

app.Run();

// Helper para converter URLs do tipo postgres:// ou postgresql:// para string de conexão compatível com Npgsql
static string ParsePostgresConnectionString(string? connStr)
{
    if (string.IsNullOrWhiteSpace(connStr))
    {
        return "Host=localhost;Port=5432;Database=royal_games;Username=postgres;Password=postgres";
    }

    if (connStr.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        connStr.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        try
        {
            var uri = new Uri(connStr);
            var userInfo = uri.UserInfo.Split(':');
            var user = userInfo.Length > 0 ? userInfo[0] : "postgres";
            var pass = userInfo.Length > 1 ? userInfo[1] : "";
            var db = uri.AbsolutePath.TrimStart('/');
            var port = uri.Port > 0 ? uri.Port : 5432;

            return $"Host={uri.Host};Port={port};Database={db};Username={user};Password={pass};TrustServerCertificate=true;";
        }
        catch
        {
            return connStr;
        }
    }

    return connStr;
}
