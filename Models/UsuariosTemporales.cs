﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

public partial class UsuariosTemporales
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Usuario { get; set; }

    [Unicode(false)]
    public string UserName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string IpMachineSolicitud { get; set; }

    [Column("fecrea", TypeName = "datetime")]
    public DateTime? Fecrea { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Active { get; set; }
}