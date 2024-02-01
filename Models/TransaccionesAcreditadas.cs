﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

public partial class TransaccionesAcreditadas
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    public int NoTransaction { get; set; }

    [Required]
    [Column("Machine_Sn")]
    [StringLength(50)]
    [Unicode(false)]
    public string MachineSn { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaTransaction { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaIni { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaFin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string NombreArchivo { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string UsuarioRegistro { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Acreditada { get; set; }
}