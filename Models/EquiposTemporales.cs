﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

public partial class EquiposTemporales
{
    public int id { get; set; }

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string serieEquipo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string IpEquipo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? fecrea { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Active { get; set; }
}