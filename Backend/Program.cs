using Backend.Data;
using Backend.Repositories;
using BackendDashboard.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
));

var jwtCfg = builder.Configuration.GetSection("Jwt");
var issuer = jwtCfg["Issuer"] ?? throw new InvalidOperationException("Missing Jwt:Issuer");
var audience = jwtCfg["Audience"] ?? throw new InvalidOperationException("Missing Jwt:Audience");

var privRel = jwtCfg["PrivateKeyPath"]
    ?? throw new InvalidOperationException("Missing Jwt:PrivateKeyPath");
var privPath = Path.Combine(builder.Environment.ContentRootPath, privRel);
if (!File.Exists(privPath))
    throw new FileNotFoundException("Private key not found", privPath);

var rsaPrivate = RSA.Create();
rsaPrivate.ImportFromPem(File.ReadAllText(privPath));
var signingKey = new RsaSecurityKey(rsaPrivate);

var pubRel = jwtCfg["PublicKeyPath"] ?? throw new InvalidOperationException("Missing Jwt:PublicKeyPath");
var pubPath = Path.Combine(builder.Environment.ContentRootPath, pubRel);

var rsaPublic = RSA.Create();
rsaPublic.ImportFromPem(File.ReadAllText(pubPath));
var validationKey = new RsaSecurityKey(rsaPublic);

builder.Services.AddSingleton<SecurityKey>(validationKey);
builder.Services.AddSingleton<RsaSecurityKey>(signingKey);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = true;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = validationKey,
            ValidateIssuer = true,
            ValidIssuer = jwtCfg["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtCfg["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

app.MapControllers();

app.Run("http://0.0.0.0:5000");
