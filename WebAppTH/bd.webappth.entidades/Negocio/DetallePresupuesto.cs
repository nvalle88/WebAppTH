using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class DetallePresupuesto
    {
        [Key]
        public int IdDetallePresupuesto { get; set; }
        public int? IdPresupuesto { get; set; }
        public int? IdSolicitudViatico { get; set; }
        public double? Valor { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Presupuesto Presupuesto{ get; set; }
        public virtual SolicitudViatico SolicitudViatico{ get; set; }
    }
}
