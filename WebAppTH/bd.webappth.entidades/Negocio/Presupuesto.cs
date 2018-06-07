using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class Presupuesto
    {
        [Key]
        public int IdPresupuesto { get; set; }
        [Display(Name = "Numero partida presupuestaria")]
        public string NumeroPartidaPresupuestaria { get; set; }
        [Display(Name = "Valor")]
        public double? Valor { get; set; }
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }
        public int? IdSucursal { get; set; }
        public virtual ICollection<DetallePresupuesto> DetallePresupuesto { get; set; }
        public virtual Sucursal Sucursal { get; set; }
    }
}
