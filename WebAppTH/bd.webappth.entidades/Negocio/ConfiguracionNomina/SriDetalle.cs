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
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal FraccionBasica { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Exceso hasta")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal ExcesoHasta { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Impuesto fracción básica")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal ImpFranccionBasica { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "% impuesto fracción básica")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P0}")]
        public decimal PorcientoImpFraccExced { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Definición del SRI")]
        public int IdSri { get; set; }
        public virtual SriNomina SriNomina { get; set; }
    }
}
