﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortalWeb_API.Models;

public partial class cuentaSignaTienda
{
    [Key]
    public int id { get; set; }

    [StringLength(30)]
    public string idtienda { get; set; }

    public int? idcuentabancaria { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? fcrea { get; set; }
}