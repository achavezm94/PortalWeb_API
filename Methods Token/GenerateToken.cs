using Microsoft.IdentityModel.Tokens;
using PortalWeb_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortalWeb_API.Methods_Token
{
    public class GenerateToken
    {
        public IConfiguration _configuration;
        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public string Generate(Usuarios_Portal usuariosPortal)
        {
            var tokenExpiration = (usuariosPortal.Rol == "R004") ? DateTime.Now.AddHours(1) : DateTime.Now.AddHours(8);
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>() ?? throw new InvalidOperationException("Jwt configuration is missing or invalid.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(ClaimTypes.Name, usuariosPortal.Usuario),
                new Claim(ClaimTypes.Role, usuariosPortal.Rol),
                new Claim(ClaimTypes.AuthorizationDecision, usuariosPortal.Active),
            };

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: tokenExpiration,
                signingCredentials: singIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
