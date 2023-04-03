using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using Sistema.Venta.BLL.Interfaces;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class ReporteVentaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVentaService _ventaServicio;

        public ReporteVentaController(IMapper mapper, IVentaService ventaServicio)
        {
            _mapper = mapper;
            _ventaServicio = ventaServicio;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]        

        public async Task<IActionResult> ReporteVenta(string fechaInicio, string fechaFin)
        {
            List<VMReporteVenta> vmLista = _mapper.Map<List<VMReporteVenta>> (await _ventaServicio.Reporte(fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
        }



    }
}
