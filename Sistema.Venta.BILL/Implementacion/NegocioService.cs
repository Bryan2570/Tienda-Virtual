using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sistema.Venta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVentas.Entity;

namespace Sistema.Venta.BLL.Implementacion
{
    public class NegocioService : INegocioServices
    {
        //SERVICIOS A UTILIZAR
        private readonly IGenericRepository<Negocio> _repositorio;
        private readonly IFirebaseService _firebaseService;

        public NegocioService(IGenericRepository<Negocio> repositorio, IFirebaseService firebaseService)
        {
            _repositorio = repositorio;
            _firebaseService = firebaseService;
        }

        public async Task<Negocio> Obtener()
        {
            try
            {
                Negocio negocio_encontrado = await _repositorio.Obtener(n => n.IdNegocio == 1);
                return negocio_encontrado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Negocio> GuardarCambios(Negocio entidad, Stream logo = null, string NombreLogo = "")
        {
            try
            {
                Negocio negocio_encontrado = await _repositorio.Obtener(n => n.IdNegocio == 1);

                negocio_encontrado.NumeroDocumento = entidad.NumeroDocumento;
                negocio_encontrado.Nombre = entidad.Nombre;
                negocio_encontrado.Correo= entidad.Correo;
                negocio_encontrado.Direccion= entidad.Direccion;
                negocio_encontrado.Telefono= entidad.Telefono;
                negocio_encontrado.PorcentajeImpuesto = entidad.PorcentajeImpuesto;
                negocio_encontrado.SimboloMoneda = entidad.SimboloMoneda;

                negocio_encontrado.NombreLogo = negocio_encontrado.NombreLogo == "" ? NombreLogo: negocio_encontrado.NombreLogo;

                if (logo != null) 
                {
                    string urlLogo = await _firebaseService.SubirStorage(logo, "carpeta_logo",negocio_encontrado.NombreLogo);
                    negocio_encontrado.UrlLogo= urlLogo;
                }
                await _repositorio.Editar(negocio_encontrado);
                return negocio_encontrado;
            }
            catch
            {

                throw;
            }
        }

       
    }
}
