using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Venta.BLL.Interfaces
{
    public interface IUtilidadesService
    {

        string GenerarClave();
        string ConvertirSha256(string texto);


    }
}
