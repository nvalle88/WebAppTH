using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class CalculoNomina
    {
        [Key]
        public int IdCalculoNomina { get; set; }

        [Display(Name = "Activo")]
        public bool EmpleadoActivo { get; set; }

        [Display(Name = "Pasivo")]
        public bool EmpleadoPasivo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        [Display(Name = "Automático")]
        public bool Automatico { get; set; }

        [Display(Name = "Reportado")]
        public bool Reportado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Periodo")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPeriodo { get; set; }
        public virtual PeriodoNomina PeriodoNomina { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Proceso")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProceso { get; set; }
        public virtual ProcesoNomina ProcesoNomina { get; set; }

        public virtual ICollection<ReportadoNomina> ReportadoNomina { get; set; }
    }
}
