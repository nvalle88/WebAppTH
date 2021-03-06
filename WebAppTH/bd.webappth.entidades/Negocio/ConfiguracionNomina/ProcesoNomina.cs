﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ProcesoNomina
    {
        public ProcesoNomina()
        {
            ConceptoNomina = new HashSet<ConceptoNomina>();
        }

        public int IdProceso { get; set; }
        
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Código")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        public virtual ICollection<ConceptoNomina> ConceptoNomina { get; set; }
        public virtual ICollection<CalculoNomina> CalculoNomina { get; set; }
    }
}
