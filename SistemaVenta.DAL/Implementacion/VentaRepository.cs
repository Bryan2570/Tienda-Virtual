using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Interfaces;
using SistemaVentas.Entity;

namespace SistemaVenta.DAL.Implementacion
{
    public class VentaRepository : GenericRepository<Ventas>, IVentaRepository
    {
        private readonly DBVENTAContext _dbContext;

        //base(dbContext) indicamos que le vamos amandar el contexto genericRepositori
        public VentaRepository(DBVENTAContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Ventas> Registrar(Ventas entidad)
        {
            Ventas ventaGenerada =  new Ventas();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in entidad.DetalleVenta)
                    {
                        Producto productoEncontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First(); //Primer producto encontrado

                        productoEncontrado.Stock = productoEncontrado.Stock - dv.Cantidad; //disminuyendo con la propiedad => Stock
                        _dbContext.Productos.Update(productoEncontrado); //actualizamos el producto encontrado con el stock disminuiodo
                    }
                    await _dbContext.SaveChangesAsync(); //guardamos lo cambios de las operaciones realizadas

                    NumeroCorrelativo correlativo = _dbContext.NumeroCorrelativos.Where(n => n.Gestion == "venta").First(); //generamos un numero correlativo

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaActualizacion = DateTime.Now;

                    _dbContext.NumeroCorrelativos.Update(correlativo);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
                    string numeroVentas = ceros + correlativo.UltimoNumero.ToString();
                    numeroVentas = numeroVentas.Substring(numeroVentas.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);
                
                    entidad.NumeroVenta = numeroVentas;

                    await _dbContext.Venta.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = entidad;

                    transaction.Commit();//si todo se cumple ya no van hacer temporales 

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return ventaGenerada;
        }

        public async Task<List<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin)
        {
            List<DetalleVenta> listaResumen = await _dbContext.DetalleVenta
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(u => u.IdUsuarioNavigation)
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(tdv => tdv.IdTipoDocumentoVentaNavigation)
                .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= FechaInicio.Date && 
                dv.IdVentaNavigation.FechaRegistro.Value.Date <= FechaFin.Date).ToListAsync();

            return listaResumen;
        }
    }
}
