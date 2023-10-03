﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

/// <summary>
/// Tabla que tiene los clientes que tienen los servicios
/// </summary>
public partial class Clientes
{
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string CodigoCliente { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string NombreCliente { get; set; }

    [Required]
    [Column("RUC")]
    [StringLength(50)]
    [Unicode(false)]
    public string Ruc { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Direccion { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Active { get; set; }

    [Column("telefcontacto")]
    [StringLength(20)]
    [Unicode(false)]
    public string Telefcontacto { get; set; }

    [Column("emailcontacto")]
    [StringLength(150)]
    [Unicode(false)]
    public string Emailcontacto { get; set; }

    [Column("nombrecontacto")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nombrecontacto { get; set; }
}