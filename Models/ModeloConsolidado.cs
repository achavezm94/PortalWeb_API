﻿#nullable disable
namespace PortalWeb_API.Models
{
    public class ModeloConsolidado
    {
        public int Tipo { get; set; }
        public string[] Equipos { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }
}