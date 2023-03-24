using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVentas.Entity;

namespace Sistema.Venta.BLL.Interfaces
{
    public interface IVentaService
    {
        Task<List<Producto>> ObtenerProducto(string busqueda);
        Task<Ventas> Registrar(Ventas entidad);
        Task<List<Ventas>> Historial(string numeroVenta, string fechaInicio, string fechaFin);
        Task<Ventas> Detalle(string numeroVenta);
        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin);
    }
}
