using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class AccionPersonalViewModel
    {
        // campos de tabla Acción Personal
        public int IdAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0} ")]
        [Display(Name = "Fecha de solicitud")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }

        
        [Display(Name = "Días restantes")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Debe introducir {0} ")]
        public string Solicitud { get; set; }

        [Required(ErrorMessage = "Debe introducir {0} ")]
        [Display(Name = "Explicación")]
        public string Explicacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0} ")]
        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaRige { get; set; }

        [Required(ErrorMessage = "Debe introducir {0} ")]
        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaRigeHasta { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int Estado { get; set; }

        
        [Display(Name = "Total días")]
        public int NoDias { get; set; }

        // Campos que no pertenecen a la tabla
        public string EstadoDirector { get; set; }
        public string EstadoValidacionTTHH { get; set; }

        //Referencias a tablas
        public DatosBasicosEmpleadoSinRequiredViewModel DatosBasicosEmpleadoViewModel { get; set; }

        [Display(Name = "Tipo de movimiento")]
        public TipoAccionesPersonalViewModel TipoAccionPersonalViewModel { get; set; }
    }
}
