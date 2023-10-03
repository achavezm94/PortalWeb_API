﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

[Table("centro_proceso")]
public partial class CentroProceso
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Nomcproceso { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string Observacion { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Ciudad { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Provincia { get; set; }

    [Column("fecrea", TypeName = "datetime")]
    public DateTime? Fecrea { get; set; }
}