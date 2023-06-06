using SistemaVentas.Entity;

namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    //Un ViewModels es practicamente lo mismo que nuestro modelo, pero solamente sacamos las propiedades que vamos a necesitar para poder interactuar con nuestra vista.
    public class VMRol
    {
        public int IdRol { get; set; }
        public string? Descripcion { get; set; }

    }
}
