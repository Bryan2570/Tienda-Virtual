using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Sistema.Venta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVentas.Entity;

namespace Sistema.Venta.BLL.Implementacion
{
    public class VentaService : IVentaService
    {
        private readonly IGenericRepository<Producto> _repositorioProducto;
        private readonly IVentaRepository _repositorioVenta;

        public VentaService(IGenericRepository<Producto> repositorioProducto, IVentaRepository repositorioVenta)
        {
            _repositorioProducto = repositorioProducto;
            _repositorioVenta = repositorioVenta;
        }


        public async Task<List<Producto>> ObtenerProducto(string busqueda)
        {
            IQueryable<Producto> query = await _repositorioProducto.Consultar(
                P => P.EsActivo == true && P.Stock > 0 && string.Concat(P.CodigoBarra, P.Marca, P.Descripcion).Contains(busqueda));

            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }

        public async Task<Ventas> Registrar(Ventas entidad)
        {
            try
            {
                return await _repositorioVenta.Registrar(entidad);
            }
            catch
            {

                throw;
            }
        }

        public async Task<List<Ventas>> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Ventas> query = await _repositorioVenta.Consultar();
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            if (fechaInicio != "" && fechaFin != "")
            {

                DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                return query.Where(v =>
                v.FechaRegistro.Value.Date >= fecha_inicio.Date &&
                v.FechaRegistro.Value.Date <= fecha_fin.Date
                )
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dv => dv.DetalleVenta)
                    .ToList();
            }
            else
            {

                return query.Where(v =>
                v.NumeroVenta == numeroVenta
                )
                   .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                   .Include(u => u.IdUsuarioNavigation)
                   .Include(dv => dv.DetalleVenta)
                   .ToList();
            }
        }

        public async Task<Ventas> Detalle(string numeroVenta)
        {
            IQueryable<Ventas> query = await _repositorioVenta.Consultar(v => v.NumeroVenta == numeroVenta);

            return query
                  .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                  .Include(u => u.IdUsuarioNavigation)
                  .Include(dv => dv.DetalleVenta)
                  .First();
        }

        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
            DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

            List<DetalleVenta> lista = await _repositorioVenta.Reporte(fecha_inicio, fecha_fin);

            return lista;

        }



    }
}
