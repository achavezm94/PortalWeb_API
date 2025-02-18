﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models
{
    public partial class Tiendas
    {
        public int id { get; set; }
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string CodigoTienda { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string CodigoClienteidFk { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string NombreTienda { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Telefono { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Direccion { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string NombreAdmin { get; set; }
        [StringLength(25)]
        [Unicode(false)]
        public string TelfAdmin { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string EmailAdmin { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string CodProv { get; set; }
        public int? idCentroProceso { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecreate { get; set; }
        [Required]
        [StringLength(1)]
        [Unicode(false)]
        public string Active { get; set; }
    }
}