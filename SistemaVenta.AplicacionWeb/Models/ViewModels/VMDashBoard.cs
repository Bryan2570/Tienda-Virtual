namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMDashBoard
    {

        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public int TotalCategorias { get; set; }

        public List<VMVentasSemana> VentasUltimaSemanas { get; set; }
        public List<VMProductoSemana> ProductosTopUltimaSemana { get; set; }

    }
}
