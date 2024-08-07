using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
#pragma warning disable CS8604 // Posible argumento de referencia nulo
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
#pragma warning restore CS8604 // Posible argumento de referencia nulo
});

builder.Services.AddAuthorization(policies =>
{
    policies.AddPolicy("Comi", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "R004");
    });
    policies.AddPolicy("Fortius", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "R001", "R002", "R003");
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

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: reglasCors, builder =>
    {
        builder.WithOrigins(
        //"https://sfifront.azurewebsites.net"
        "http://192.168.55.66:2251",
        "http://192.168.55.19:2253"
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

//webSocketOptions.AllowedOrigins.Add("https://sfifront.azurewebsites.net");
webSocketOptions.AllowedOrigins.Add("http://192.168.55.19:2253");

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