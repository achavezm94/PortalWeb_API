﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalWeb_API.Models
{
    public partial class ObtenerTiendasResult
    {
        public int? cantidadMaquinaria { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string RUC { get; set; }
        public string Telefono { get; set; }
        public string NombreAdmin { get; set; }
        public string TelfAdmin { get; set; }
        public string Direccion { get; set; }
        public string nombreLocalidad { get; set; }
        public int id { get; set; }
        public string CodigoTienda { get; set; }
        public string CodigoClienteidFk { get; set; }
        public string NombreTienda { get; set; }
        public string EmailAdmin { get; set; }
        public string CodProv { get; set; }
        public int? idCentroProceso { get; set; }
        public DateTime? fecreate { get; set; }
        public string Active { get; set; }
        public int? cantidadCuentasAsign { get; set; }
    }
}