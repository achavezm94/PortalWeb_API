using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalWeb_API;
using PortalWeb_API.Data;
using PortalWeb_APIs;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PortalWebContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//no valida del lado del servidor
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(policies =>
{
    policies.AddPolicy("User", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "user");
    });
    policies.AddPolicy("Admin", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "admin");
    });
    policies.AddPolicy("All", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "admin", "user");
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var reglasCors = "ReglasCors";

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
IServiceCollection serviceCollection = builder.Services.AddDbContext<PortalWebContext>(opt => opt.UseInMemoryDatabase(databaseName: "PortalWeb"));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024;
    //options.HandshakeTimeout = TimeSpan.FromSeconds(5);
    //options.KeepAliveInterval = TimeSpan.FromSeconds(10);
});

//http://192.168.100.10:2251
//https://sfifront.azurewebsites.net
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: reglasCors, builder =>
    {
        builder.WithOrigins(
        "http://192.168.100.10:2251"
    )
   .AllowAnyHeader()
   .AllowAnyMethod()
   .AllowCredentials();
    });
});

var webSocketOptions = new WebSocketOptions
{    
    KeepAliveInterval = TimeSpan.FromSeconds(120)
};

webSocketOptions.AllowedOrigins.Add("http://192.168.100.10:2251");

var app = builder.Build();
app.UseCors(reglasCors);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.UseWebSockets(webSocketOptions);

// TUNELES HUB DE COMUNICACION
#region
app.MapHub<PingHubEquipos>("/hubs/PingHubEquipos");
app.MapHub<ManualesHub>("/hubs/manualTransaction");
app.MapHub<AutomaticoTransaHUb>("/hubs/autoTransaccion");
app.MapHub<RecoleccionHub>("/hubs/recoleccionTransaccion");
app.MapHub<UsuarioHub>("/hubs/usuarioTemporal");
app.MapHub<EliminarUsuario>("/hubs/eliminarTemporal");
#endregion

app.Run();