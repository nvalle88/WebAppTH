using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class AdicionarReportadoNominaViewModel
    {
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Empleado")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Rubro")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdConcepto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Valor")]
        [DataType(DataType.Currency)]
        public double Valor { get; set; }


        public int IdCalculoNomina { get; set; }
    }
}
