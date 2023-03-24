using System;
using System.Collections.Generic;

namespace SistemaVentas.Entity
{
    public partial class TipoDocumentoVenta
    {
        public TipoDocumentoVenta()
        {
            Venta = new HashSet<Ventas>();
        }

        public int IdTipoDocumentoVenta { get; set; }
        public string? Descripcion { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<Ventas> Venta { get; set; }
    }
}
