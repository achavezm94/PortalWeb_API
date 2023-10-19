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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var reglasCors = "ReglasCors";

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
IServiceCollection serviceCollection = builder.Services.AddDbContext<PortalWebContext>(opt => opt.UseInMemoryDatabase(databaseName: "PortalWeb"));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024;
});


builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: reglasCors, builder =>
    {
        builder.WithOrigins(
        "https://sfifront.azurewebsites.net"
    )
   .AllowAnyHeader()
   .AllowAnyMethod()
   .AllowCredentials();
    });
});
/**/
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

webSocketOptions.AllowedOrigins.Add("https://sfifront.azurewebsites.net");

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
#endregion

app.Run();


/*
 app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<PingHubEquipos>("/hubs/PingHubEquipos");
    });
 */