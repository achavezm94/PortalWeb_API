﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

public partial class Usuarios
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Usuario { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Contrasenia { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string TiendasidFk { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Active { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string IpMachine { get; set; }

    public int? CuentasidFk { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Observacion { get; set; }
}