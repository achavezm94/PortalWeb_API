namespace PortalWeb_API.Models
{
    public class TotalTodoTransacciones
    {
        public double? Total { get; set; }
    }

    public class TotalUltimaTransaccion
    {
        public int? TotalCantMonedas { get; set; }
        public int? TotalCantBilletes { get; set; }
        public double? TotalMont { get; set; }
    }
}
