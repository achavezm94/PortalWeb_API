﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models
{
    public partial class cuentas_bancarias
    {
        public int id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string CodigoCliente { get; set; }
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string codcuentacontable { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string nombanco { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string numerocuenta { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string TipoCuenta { get; set; }
        [Unicode(false)]
        public string Observacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecrea { get; set; }
    }
}