using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ConceptoConjuntoNomina
    {
        public int IdConceptoConjunto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Conjunto")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdConjunto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Concepto")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdConcepto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Suma")]

        public bool Suma { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Resta")]
        public bool Resta { get; set; }

        public virtual ConceptoNomina Concepto { get; set; }
        public virtual ConjuntoNomina Conjunto { get; set; }
    }
}
