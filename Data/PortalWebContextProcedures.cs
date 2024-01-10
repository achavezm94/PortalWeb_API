﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
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
    public partial class PortalWebContext
    {
        private IPortalWebContextProcedures _procedures;

        public virtual IPortalWebContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new PortalWebContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IPortalWebContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObtenerClientesResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_CalculoTotalResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_DatosEquiposFrontResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_FiltroPorFechaTransaccionesResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GraficoTotalesAñoResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_IndicadoresLlenadoResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<sp_obtener_maquinariaResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_ObtenerEquipoTempResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_TablaTransaccionalPrincipalResult>().HasNoKey().ToView(null);
        }
    }

    public partial class PortalWebContextProcedures : IPortalWebContextProcedures
    {
        private readonly PortalWebContext _context;

        public PortalWebContextProcedures(PortalWebContext context)
        {
            _context = context;
        }

        public virtual async Task<List<ObtenerClientesResult>> ObtenerClientesAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ObtenerClientesResult>("EXEC @returnValue = [dbo].[ObtenerClientes]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_CalculoTotalResult>> SP_CalculoTotalAsync(string machine_sn, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "machine_sn",
                    Size = 100,
                    Value = machine_sn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_CalculoTotalResult>("EXEC @returnValue = [dbo].[SP_CalculoTotal] @machine_sn", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_DatosEquiposFrontResult>> SP_DatosEquiposFrontAsync(string id_equipo, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "id_equipo",
                    Size = 50,
                    Value = id_equipo ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_DatosEquiposFrontResult>("EXEC @returnValue = [dbo].[SP_DatosEquiposFront] @id_equipo", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_FiltroPorFechaTransaccionesResult>> SP_FiltroPorFechaTransaccionesAsync(int? tipo, string id_tienda, DateTime? fechaInicial, DateTime? fechaFinal, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "tipo",
                    Value = tipo ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "id_tienda",
                    Size = 100,
                    Value = id_tienda ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                new SqlParameter
                {
                    ParameterName = "fechaInicial",
                    Value = fechaInicial ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "fechaFinal",
                    Value = fechaFinal ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_FiltroPorFechaTransaccionesResult>("EXEC @returnValue = [dbo].[SP_FiltroPorFechaTransacciones] @tipo, @id_tienda, @fechaInicial, @fechaFinal", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GraficoTotalesAñoResult>> SP_GraficoTotalesAñoAsync(string serieEquipo, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "serieEquipo",
                    Size = 50,
                    Value = serieEquipo ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GraficoTotalesAñoResult>("EXEC @returnValue = [dbo].[SP_GraficoTotalesAño] @serieEquipo", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_IndicadoresLlenadoResult>> SP_IndicadoresLlenadoAsync(string idTienda, int? type, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "idTienda",
                    Size = 100,
                    Value = idTienda ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                new SqlParameter
                {
                    ParameterName = "type",
                    Value = type ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_IndicadoresLlenadoResult>("EXEC @returnValue = [dbo].[SP_IndicadoresLlenado] @idTienda, @type", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<sp_obtener_maquinariaResult>> sp_obtener_maquinariaAsync(int? type, string codTienda, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "type",
                    Value = type ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "codTienda",
                    Size = 50,
                    Value = codTienda ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<sp_obtener_maquinariaResult>("EXEC @returnValue = [dbo].[sp_obtener_maquinaria] @type, @codTienda", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_ObtenerEquipoTempResult>> SP_ObtenerEquipoTempAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_ObtenerEquipoTempResult>("EXEC @returnValue = [dbo].[SP_ObtenerEquipoTemp]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_TablaTransaccionalPrincipalResult>> SP_TablaTransaccionalPrincipalAsync(string id_tienda, int? type, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "id_tienda",
                    Size = 100,
                    Value = id_tienda ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                new SqlParameter
                {
                    ParameterName = "type",
                    Value = type ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_TablaTransaccionalPrincipalResult>("EXEC @returnValue = [dbo].[SP_TablaTransaccionalPrincipal] @id_tienda, @type", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
