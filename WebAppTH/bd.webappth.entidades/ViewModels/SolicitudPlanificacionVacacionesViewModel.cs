using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class SolicitudPlanificacionVacacionesViewModel
    {
        public DatosBasicosEmpleadoViewModel DatosBasicosEmpleadoViewModel { get; set; }

        public int IdSolicitudPlanificacionVacaciones { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaSolicitud { get; set; }

        [Display(Name = "Solicita desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaDesde { get; set; }

        [Display(Name = "Solicita hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaHasta { get; set; }

        public int Estado { get; set; }

        [Display(Name = "Estado")]
        public string NombreEstado { get; set; }


        public string Observaciones { get; set; }

        [Display(Name = "Vacaciones acumuladas")]
        public decimal VacacionesAcumuladas { get; set; }
    }
}
