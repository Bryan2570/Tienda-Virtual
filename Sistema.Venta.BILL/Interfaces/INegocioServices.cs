using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVentas.Entity;

namespace Sistema.Venta.BLL.Interfaces
{
    public interface INegocioServices
    {
        Task<Negocio> Obtener();
        Task<Negocio> GuardarCambios(Negocio entidad, Stream logo = null, string NombreLogo = "");


    }
}
