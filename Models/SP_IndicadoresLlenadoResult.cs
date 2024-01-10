﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalWeb_API.Models
{
    public partial class SP_IndicadoresLlenadoResult
    {
        public string NombreTienda { get; set; }
        public string TelfAdmin { get; set; }
        public string TipoMaquinaria { get; set; }
        public string NombreMarca { get; set; }
        public string NombreModelo { get; set; }
        public int? IdEquipo { get; set; }
        public int? CodigoTiendaidFk { get; set; }
        public int? Tipo { get; set; }
        public int? Marca { get; set; }
        public int? Modelo { get; set; }
        public int? TotalMaxAsegurado { get; set; }
        public string SerieEquipo { get; set; }
        public string Active { get; set; }
        public int? CapacidadMaximaBilletes { get; set; }
        public DateTime? FechaInstalacion { get; set; }
        public int? EstadoPing { get; set; }
        public DateTime? TiempoSincronizacion { get; set; }
        public DateTime? Fecrea { get; set; }
        public int? CapacidadMaximoPesos { get; set; }
        public string IpEquipo { get; set; }
        public int? idEquipo { get; set; }
        public DateTime? ultimRecoleccion { get; set; }
        public int? CantidadSobres { get; set; }
        public int? CapacidadBilletes { get; set; }
        [Column("CapacidadPesos", TypeName = "decimal(10,4)")]
        public decimal? CapacidadPesos { get; set; }
        [Column("TotalAsegurado", TypeName = "decimal(10,2)")]
        public decimal? TotalAsegurado { get; set; }
    }
}
