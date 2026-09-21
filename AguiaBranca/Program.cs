using AguiaBranca.Applications.Autenticacao;
using AguiaBranca.Applications.Services;
using AguiaBranca.Context;
using AguiaBranca.Interfaces;
using AguiaBranca.Repositories.MongoDB;
using AguiaBranca.Repositories.InMemory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Cole aqui o token JWT (sem a palavra 'Bearer', o Swagger adiciona sozinho)"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var useInMemory = builder.Configuration.GetValue<bool>("DatabaseSettings:UseInMemory");

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<InMemoryDbContext>();

if (useInMemory)
{
    builder.Services.AddScoped<IEstrategiaRepository, EstrategiaInMemoryRepository>();
    builder.Services.AddScoped<IIdeiaInovacaoRepository, IdeiaInovacaoInMemoryRepository>();
    builder.Services.AddScoped<IProjetoRepository, ProjetoInMemoryRepository>();
    builder.Services.AddScoped<IUsuarioRepository, UsuarioInMemoryRepository>();
}
else
{
    builder.Services.AddScoped<IEstrategiaRepository, EstrategiaMongoRepository>();
    builder.Services.AddScoped<IIdeiaInovacaoRepository, IdeiaInovacaoMongoRepository>();
    builder.Services.AddScoped<IProjetoRepository, ProjetoMongoRepository>();
    builder.Services.AddScoped<IUsuarioRepository, UsuarioMongoRepository>();
}


builder.Services.AddScoped<EstrategiaService>();
builder.Services.AddScoped<IdeiaInovacaoService>();
builder.Services.AddScoped<ProjetoService>();
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<DashboardService>();


builder.Services.AddScoped<Criptografia>();
builder.Services.AddScoped<GeradorTokenJWT>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();