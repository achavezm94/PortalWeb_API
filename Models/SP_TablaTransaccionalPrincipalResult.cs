﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalWeb_API.Models
{
    public partial class SP_TablaTransaccionalPrincipalResult
    {
        public DateTime? FechaTransaccion { get; set; }
        public string NombreCliente { get; set; }
        public string NombreTienda { get; set; }
        public int? Transaccion_No { get; set; }
        public string Machine_Sn { get; set; }
        public string Usuarios_idFk { get; set; }
        public string Establecimiento { get; set; }
        public string CodigoEstablecimiento { get; set; }
        public string nombanco { get; set; }
        public string TipoCuenta { get; set; }
        public string numerocuenta { get; set; }
        public string Observacion { get; set; }
        public int? Deposito_Bill_1 { get; set; }
        public int? Deposito_Bill_2 { get; set; }
        public int? Deposito_Bill_5 { get; set; }
        public int? Deposito_Bill_10 { get; set; }
        public int? Deposito_Bill_20 { get; set; }
        public int? Deposito_Bill_50 { get; set; }
        public int? Deposito_Bill_100 { get; set; }
        public int? Manual_Deposito_Coin_1 { get; set; }
        public int? Manual_Deposito_Coin_5 { get; set; }
        public int? Manual_Deposito_Coin_10 { get; set; }
        public int? Manual_Deposito_Coin_25 { get; set; }
        public int? Manual_Deposito_Coin_50 { get; set; }
        public int? Manual_Deposito_Coin_100 { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalRecoleccion { get; set; }
        public string TipoTransaccion { get; set; }
        public DateTime? FechaRecoleccion { get; set; }
    }
}
