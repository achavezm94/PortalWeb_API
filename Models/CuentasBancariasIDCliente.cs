using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalWeb_API.Models
{
    public class CuentasBancariasIDCliente
    {
        public int Id { get; set; }
        public int ClienteID { get; set; }
        public string? CodigoCliente { get; set; }

        public string? Codcuentacontable { get; set; }

        public string? Nombanco { get; set; }

        public string? Numerocuenta { get; set; }

        public string? TipoCuenta { get; set; }

        public string? Observacion { get; set; }

        public DateTime? Fecrea { get; set; }
    }
}
