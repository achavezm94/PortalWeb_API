﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

[Keyless]
public partial class MasterTable
{
    [Column("master")]
    [StringLength(5)]
    [Unicode(false)]
    public string Master { get; set; }

    [Required]
    [Column("codigo")]
    [StringLength(20)]
    [Unicode(false)]
    public string Codigo { get; set; }

    [Column("nombre")]
    [StringLength(200)]
    public string Nombre { get; set; }

    [Column("valor", TypeName = "decimal(15, 2)")]
    public decimal? Valor { get; set; }

    [Column("nomtag")]
    [StringLength(10)]
    [Unicode(false)]
    public string Nomtag { get; set; }

    [Column("gestion")]
    [StringLength(3)]
    [Unicode(false)]
    public string Gestion { get; set; }

    [Column("pideval")]
    public bool Pideval { get; set; }

    [Column("campo1")]
    [StringLength(30)]
    [Unicode(false)]
    public string Campo1 { get; set; }

    [Column("grupo")]
    [StringLength(70)]
    public string Grupo { get; set; }

    [Column("sgrupo")]
    [StringLength(70)]
    [Unicode(false)]
    public string Sgrupo { get; set; }

    [Column("campo2")]
    [StringLength(30)]
    [Unicode(false)]
    public string Campo2 { get; set; }

    [Column("lencod", TypeName = "decimal(2, 0)")]
    public decimal Lencod { get; set; }

    [Column("VALOR2", TypeName = "decimal(16, 2)")]
    public decimal? Valor2 { get; set; }
}