using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                ValidAudience = builder.Configuration["Jwt:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt configuration is missing or invalid.")))
                            };
                        }
);

builder.Services.AddAuthorization(policies =>
{
    policies.AddPolicy("Comi", p => p.RequireClaim(ClaimTypes.Role, "R004"));
    policies.AddPolicy("Monitor", p => p.RequireClaim(ClaimTypes.Role, "R005", "R001", "R002", "R003", "R006"));
    policies.AddPolicy("Transaccional", p => p.RequireClaim(ClaimTypes.Role, "R005", "R001", "R002", "R006"));
    policies.AddPolicy("Nivel1", p => p.RequireClaim(ClaimTypes.Role, "R001"));
    policies.AddPolicy("Nivel2", p => p.RequireClaim(ClaimTypes.Role, "R001", "R002"));
    policies.AddPolicy("Nivel3", p => p.RequireClaim(ClaimTypes.Role, "R001", "R002", "R003"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API RESTful",
        Version = "v1",
        Description = "API para aplicacion FortiCash.",
        Contact = new OpenApiContact
        {
            Name = "Adrian Chavez",
            Email = "achavez@doriantrade.com",
        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese 'Bearer' [espacio] y luego el token JWT.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});
var reglasCors = "ReglasCors";

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
IServiceCollection serviceCollection = builder.Services.AddDbContext<PortalWebContext>(opt => opt.UseInMemoryDatabase(databaseName: "PortalWeb"));

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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
        //"https://sfifront.azurewebsites.net"
        "http://192.168.55.236:2251",
        "http://192.168.55.19:2253",
        "http://192.168.55.212:2251"
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
webSocketOptions.AllowedOrigins.Add("http://192.168.55.236:2251");
webSocketOptions.AllowedOrigins.Add("http://192.168.55.19:2253");
webSocketOptions.AllowedOrigins.Add("http://192.168.55.212:2251");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API RESTful v1");
        c.RoutePrefix = "swagger";
    });
}


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
app.MapHub<UsuarioHub>("/hubs/usuarioTemporal");
#endregion

app.Run();