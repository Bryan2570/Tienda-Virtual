﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVentas.Entity;


namespace SistemaVenta.DAL.Interfaces
{
    public interface IVentaRepository : IGenericRepository<Ventas>
    {
        Task<Ventas> Registrar(Ventas entidad);
        Task<List<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin);
    }
}
