﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalWeb_API.Models
{
    public partial class ObtenerClientesResult
    {
        public int id { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string Active { get; set; }
        public string telefcontacto { get; set; }
        public string emailcontacto { get; set; }
        public string nombrecontacto { get; set; }
        public int? cantidadCuntasBancarias { get; set; }
    }
}
