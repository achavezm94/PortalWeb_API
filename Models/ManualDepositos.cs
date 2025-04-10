﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models
{
    public partial class ManualDepositos
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Usuarios_idFk { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Machine_Sn { get; set; }
        public int Transaccion_No { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaTransaccion { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string DivisaTransaccion { get; set; }
        public int? Manual_Deposito_Bill_1 { get; set; }
        public int? Manual_Deposito_Bill_2 { get; set; }
        public int? Manual_Deposito_Bill_5 { get; set; }
        public int? Manual_Deposito_Bill_10 { get; set; }
        public int? Manual_Deposito_Bill_20 { get; set; }
        public int? Manual_Deposito_Bill_50 { get; set; }
        public int? Manual_Deposito_Bill_100 { get; set; }
        public int? Manual_Deposito_Coin_1 { get; set; }
        public int? Manual_Deposito_Coin_5 { get; set; }
        public int? Manual_Deposito_Coin_10 { get; set; }
        public int? Manual_Deposito_Coin_25 { get; set; }
        public int? Manual_Deposito_Coin_50 { get; set; }
        public int? Manual_Deposito_Coin_100 { get; set; }
        public int? Total_Deposito_Bill_1 { get; set; }
        public int? Total_Deposito_Bill_2 { get; set; }
        public int? Total_Deposito_Bill_5 { get; set; }
        public int? Total_Deposito_Bill_10 { get; set; }
        public int? Total_Deposito_Bill_20 { get; set; }
        public int? Total_Deposito_Bill_50 { get; set; }
        public int? Total_Deposito_Bill_100 { get; set; }
        public int? Total_Manual_Deposito_Bill_1 { get; set; }
        public int? Total_Manual_Deposito_Bill_2 { get; set; }
        public int? Total_Manual_Deposito_Bill_5 { get; set; }
        public int? Total_Manual_Deposito_Bill_10 { get; set; }
        public int? Total_Manual_Deposito_Bill_20 { get; set; }
        public int? Total_Manual_Deposito_Bill_50 { get; set; }
        public int? Total_Manual_Deposito_Bill_100 { get; set; }
        public int? Total_Manual_Deposito_Coin_1 { get; set; }
        public int? Total_Manual_Deposito_Coin_5 { get; set; }
        public int? Total_Manual_Deposito_Coin_10 { get; set; }
        public int? Total_Manual_Deposito_Coin_25 { get; set; }
        public int? Total_Manual_Deposito_Coin_50 { get; set; }
        public int? Total_Manual_Deposito_Coin_100 { get; set; }
        [Required]
        [StringLength(1)]
        [Unicode(false)]
        public string Active { get; set; }
    }
}