﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PortalWeb_API.Data
{
    public partial interface IPortalWebContextProcedures
    {
        Task<List<SP_ConsolidadoCajasResult>> SP_ConsolidadoCajasAsync(string tipo, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_EquiposNoTransaccionesResult>> SP_EquiposNoTransaccionesAsync(DateTime? fechaInicial, DateTime? fechaFinal, int? opcion, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_FiltroPorFechaComisariatoResult>> SP_FiltroPorFechaComisariatoAsync(string id_tienda, DateTime? fechaInicial, DateTime? fechaFinal, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_FiltroPorFechaTransaccionesResult>> SP_FiltroPorFechaTransaccionesAsync(int? tipo, string id_tienda, DateTime? fechaInicial, DateTime? fechaFinal, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
