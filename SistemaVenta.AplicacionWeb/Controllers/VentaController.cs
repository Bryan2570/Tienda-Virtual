using Microsoft.AspNetCore.Mvc;


using AutoMapper;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using Sistema.Venta.BLL.Interfaces;
using SistemaVentas.Entity;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class VentaController : Controller
    {
        private readonly ITipoDocumentoVentaService _tipoDocumentoVentaServicio;
        private readonly IVentaService _ventaServicio;
        private readonly IMapper _mapper;

        public VentaController(ITipoDocumentoVentaService tipoDocumentoVentaServicio, IVentaService ventaServicio, IMapper mapper)
        {
            _tipoDocumentoVentaServicio = tipoDocumentoVentaServicio;
            _ventaServicio = ventaServicio;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            List<VMTipoDocumentoVenta> vmListaTipoDocumento = _mapper.Map<List<VMTipoDocumentoVenta>>(await _tipoDocumentoVentaServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, vmListaTipoDocumento);
        }


        [HttpGet]
        //mapper convertir producto a vmproducto
        public async Task<IActionResult> ObtenerProducto(string busqueda)
        {
            List<VMProducto> vmListaProductos = _mapper.Map<List<VMProducto>>(await _ventaServicio.ObtenerProducto(busqueda));

            return StatusCode(StatusCodes.Status200OK, vmListaProductos);
        }



        [HttpPost]

        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta> gResponse = new GenericResponse<VMVenta>();

            try
            {
                modelo.IdUsuario = 21;

                Ventas venta_creada = await _ventaServicio.Registrar(_mapper.Map<Ventas>(modelo));
                modelo = _mapper.Map<VMVenta>(venta_creada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpGet]

        public async Task<IActionResult> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            List<VMVenta> vmHistorialVenta = _mapper.Map<List<VMVenta>>(await _ventaServicio.Historial(numeroVenta, fechaInicio, fechaFin));

            return StatusCode(StatusCodes.Status200OK, vmHistorialVenta);
        }
    }
}
