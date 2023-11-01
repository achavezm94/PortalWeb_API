﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

[Table("Datos_Personales")]
public partial class DatosPersonales
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UsuarioPortaidFk { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UsuarioidFk { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Apellidos { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Cedula { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Telefono { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Active { get; set; }
}