using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class InduccionViewModel
    {
        public int IdEmpleado { get; set; }
        public string Nombres { get; set; }

        [Display(Name = "Fecha de ingreso a la institución")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Estado")]
        public string EstadoInduccion { get; set; }

        public string ValorCompletado { get; set; }
    }
}
