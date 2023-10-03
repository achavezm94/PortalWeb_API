﻿using Microsoft.AspNetCore.Mvc;
using PortalWeb_API.Data;
using PortalWeb_API.Methods_Token;
using PortalWeb_API.Models;


namespace PortalWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        
        public IConfiguration _configuration;
        private readonly PortalWebContext _context;
        public LoginController(PortalWebContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public IActionResult Login(UserRequest userRequest) 
        {
            Authenticator authenticator = new(_context);
            GenerateToken generateToken = new(_configuration);
            var user = authenticator.Authenticator_User(userRequest);
            if (user != null) 
            {
                var token = generateToken.Generate(user);
                return Ok(token);
            }
            else
            {
                return NotFound("Usuario no registrado");
            }

        }
    }
}
