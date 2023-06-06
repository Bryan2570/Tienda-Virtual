namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMPDFVenta
    {
        //utilizamos otros modelos como si fueran propiedades
        public VMNegocio? negocio { get; set; }
        public VMVenta? venta { get; set; }

    }
}
