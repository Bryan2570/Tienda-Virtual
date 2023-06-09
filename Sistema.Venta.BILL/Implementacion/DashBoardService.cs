using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Sistema.Venta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVentas.Entity;
using System.Globalization;

namespace Sistema.Venta.BLL.Implementacion
{
    public class DashBoardService : IDashBoardService
    {

        private readonly IVentaRepository _repositorioVenta;
        private readonly IGenericRepository<DetalleVenta> _repositorioDetalleVenta;
        private readonly IGenericRepository<Categoria> _repositorioCategoria;
        private readonly IGenericRepository<Producto> _repositorioProducto;
        private DateTime FechaInicio = DateTime.Now;


        public DashBoardService(IVentaRepository repositorioVenta, IGenericRepository<DetalleVenta> repositorioDetalleVenta, IGenericRepository<Categoria> repositorioCategoria, IGenericRepository<Producto> repositorioProducto)
        {
            _repositorioVenta = repositorioVenta;
            _repositorioDetalleVenta = repositorioDetalleVenta;
            _repositorioCategoria = repositorioCategoria;
            _repositorioProducto= repositorioProducto;

            FechaInicio = FechaInicio.AddDays(-7);
        }

        public async Task<int> TotalVentasUltimaSemana()
        {
            try
            {
                IQueryable<Ventas> query = await _repositorioVenta.Consultar(v => v.FechaRegistro.Value.Date >= FechaInicio.Date);
                int total = query.Count();
                return total;                    
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> TotalIngresosUltimaSemana()
        {
            try
            {
                IQueryable<Ventas> query = await _repositorioVenta.Consultar(v => v.FechaRegistro.Value.Date >= FechaInicio.Date);
                decimal resultado = query
                    .Select(v => v.Total)
                    .Sum(v => v.Value);

                return Convert.ToString(resultado, new CultureInfo("es-CO"));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> TotalProductos()
        {
            try
            {
                IQueryable<Producto> query = await _repositorioProducto.Consultar();
                int total = query.Count();
                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> TotalCategorias()
        {
            try
            {
                IQueryable<Categoria> query = await _repositorioCategoria.Consultar();
                int total = query.Count();
                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            try
            {
                IQueryable<Ventas> query = await _repositorioVenta
                .Consultar(v => v.FechaRegistro.Value.Date >= FechaInicio.Date);

                Dictionary<string, int> resultado = query
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderByDescending(g => g.Key) //ORDENAMOS POR EL VALOR DE GROUP BY
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }) //CREAMOS UN NUEVO OBEJTO CON LA PROPIEDAD DE FECHA Y TOTAL
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<Dictionary<string, int>> ProductosTopUltimaSemana()
        {
            try
            {
                IQueryable<DetalleVenta> query = await _repositorioDetalleVenta.Consultar();

                Dictionary<string, int> resultado = query
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= FechaInicio.Date)
                    .GroupBy(dv => dv.DescripcionProducto).OrderByDescending(g => g.Count())
                    .Select(dv => new { producto = dv.Key, total = dv.Count() }).Take(4)  //Take(4) >> decimos que vamos a tener cuatro elementos 4 filas
                    .ToDictionary(keySelector: r => r.producto, elementSelector: r => r.total);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        

      

       
    }
}
