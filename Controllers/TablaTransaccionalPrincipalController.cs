﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Data;
using System.Data;

namespace PortalWeb_API.Controllers
{
    [Route("api/Transacciones")]
    [ApiController]
    public class TablaTransaccionalPrincipalController : ControllerBase
    {
        private readonly PortalWebContext _context;

        public TablaTransaccionalPrincipalController(PortalWebContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerTransacciones/{id}")]
        public IActionResult ObtenerTransacciones([FromRoute] int id)
        {
            string Sentencia = "SP_TablaTransaccionalPrincipal "+@id;
            DataTable dt = new();
            using (SqlConnection connection = new(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new(Sentencia, connection);
                SqlDataAdapter adapter = new(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);
        }
    }
}
