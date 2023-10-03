using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalWeb_API.Models
{
    public class EquipoTienda
    {
        public int Id { get; set; }

        public string? NombreTienda { get; set; }

        public string? TipoId { get; set; }

        public string? MarcaId { get; set; }

        public string? ModeloId { get; set; }
        public string? Tipo { get; set; }

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string? SerieEquipo { get; set; }

        public string? Active { get; set; }

        public int? CapacidadIni { get; set; }

        public DateTime? FechaInstalacion { get; set; }

        public int? EstadoPing { get; set; }

        public int? CapacidadFin { get; set; }

        public DateTime? TiempoSincronizacion { get; set; }

        public DateTime? Fecrea { get; set; }

        public string? Provincia { get; set; }
    }
}
