using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bd.webappth.entidades.Negocio
{
    public partial class ReliquidacionViatico
    {
        
        [NotMapped]
        public decimal ValorTotalRequlidacion { get; set; }
        [NotMapped]
        public decimal ValorRequlidacion { get; set; }        
        [NotMapped]
        public int IdEmpleado { get; set; }

        public int IdReliquidacionViatico { get; set; }
        public int IdSolicitudViatico { get; set; }
        public string Descripcion { get; set; }
        public decimal? ValorEstimado { get; set; }
        public bool Estado { get; set; }
        public int? IdPresupuesto { get; set; }

        public virtual ICollection<DetalleReliquidacionViatico> DetalleReliquidacionViatico { get; set; }
        public virtual Presupuesto Presupuesto { get; set; }
        public virtual SolicitudViatico SolicitudViatico { get; set; }
    }
}
