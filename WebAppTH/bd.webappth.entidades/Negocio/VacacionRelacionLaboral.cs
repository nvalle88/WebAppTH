using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class VacacionRelacionLaboral
    {
        public int IdVacacionRelacionLaboral { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Relación laboral")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar {0} ")]
        public int IdRegimenLaboral { get; set; }

        [Display(Name = "Mínimo de días")]
        public int MinAcumulacionDias { get; set; }

        [Display(Name = "Máximo de días")]
        public int MaxAcumulacionDias { get; set; }

        [Display(Name = "Incremento días por período fiscal")]
        public int IncrementoDiasPorPeriodoFiscal { get; set; }

        [Display(Name = "Incrementar a partir del año")]
        public int IncrementoApartirPeriodoFiscal { get; set; }
        
        public virtual RegimenLaboral RegimenLaboral { get; set; }
    }
}
