namespace PortalWeb_API.Models
{
    internal class ClienteDto
    {
        public string? CodigoCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? RUC { get; set; }
        public string? Direccion { get; set; }
        public string? Active { get; set; }
        public string? Telefcontacto { get; set; }
        public string? Emailcontacto { get; set; }
        public string? Nombrecontacto { get; set; }
        public List<CuentasBancarias>? CuentasBancarias { get; set; }
    }
}