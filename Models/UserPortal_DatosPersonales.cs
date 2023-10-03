namespace PortalWeb_API.Models
{
    public class UserPortal_DatosPersonales
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public string? Contrasenia { get; set; }
        public string? Rol { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Cedula { get; set; }
        public string? Telefono { get; set; }
        public string? Active { get; set; }
    }
}
