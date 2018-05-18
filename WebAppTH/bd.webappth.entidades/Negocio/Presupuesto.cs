using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class Presupuesto
    {
        [Key]
        public int IdPresupuesto { get; set; }
        public string NumeroPartidaPresupuestaria { get; set; }
        public double? Valor { get; set; }
        public DateTime? Fecha { get; set; }
        public virtual ICollection<DetallePresupuesto> DetallePresupuesto { get; set; }
    }
}
