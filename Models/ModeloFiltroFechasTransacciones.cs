namespace PortalWeb_API.Models
{
    public class ModeloFiltroFechasTransacciones
    {
        public int Tipo { get; set; }
        public string? Machine_Sn { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
