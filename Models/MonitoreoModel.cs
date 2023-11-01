namespace PortalWeb_API.Models
{
    public class MonitoreoModel
    {
        public string? ip { get; set; }
        public DateTime? tiempoSincronizacion { get; set; }
        public int estadoPing { get; set; }
    }
}
