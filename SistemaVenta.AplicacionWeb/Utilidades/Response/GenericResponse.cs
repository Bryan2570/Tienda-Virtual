namespace SistemaVenta.AplicacionWeb.Utilidades.Response
{
    public class GenericResponse<TObjet>
    {
        public bool Estado { get; set; }
        public string? Mensaje { get; set; }
        public TObjet? Objeto { get; set; }
        public List<TObjet>? ListaObjeto { get; set; }


    }
}
