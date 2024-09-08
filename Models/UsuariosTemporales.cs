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
    public int id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Usuario { get; set; }

    [Unicode(false)]
    public string UserName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string IpMachineSolicitud { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? fecrea { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Active { get; set; }
}