﻿using Microsoft.Extensions.Configuration;
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
            _configuration = configuration;
        }
        public string Generate(UsuariosPortal usuariosPortal)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(ClaimTypes.NameIdentifier, usuariosPortal.Id.ToString()),
                new Claim(ClaimTypes.Name, usuariosPortal.Usuario),
                new Claim(ClaimTypes.Role, usuariosPortal.Rol),
                new Claim(ClaimTypes.AuthorizationDecision, usuariosPortal.Active),
            };

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: singIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
