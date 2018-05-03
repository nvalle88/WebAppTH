using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class SriDetalle
    {
        [Key]
        public int IdSriDetalle { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fracción básica")]
        public double FraccionBasica { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Exceso hasta")]
        public double ExcesoHasta { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Impuesto fracción básica")]
        public double ImpFranccionBasica { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "% impuesto fracción básica")]
        public double PorcientoImpFraccExced { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Definición del SRI")]
        public int IdSri { get; set; }
        public virtual SriNomina SriNomina { get; set; }
    }
}
