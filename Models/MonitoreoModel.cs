namespace PortalWeb_API.Models
{
    public class MonitoreoModel
    {
        public string? Ip { get; set; }
        public DateTime? TiempoSincronizacion { get; set; }
        public int EstadoPing { get; set; }
    }
}
