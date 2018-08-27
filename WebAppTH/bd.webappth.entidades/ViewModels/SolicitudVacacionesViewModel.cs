using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.ViewModels
{
    public class SolicitudVacacionesViewModel
    {
        public DatosBasicosEmpleadoViewModel DatosBasicosEmpleadoViewModel { get; set; }

        public int IdSolicitudVacaciones { get; set; }

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

        [Display(Name = "Fecha respuesta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaRespuesta { get; set; }

        public bool PlanAnual { get; set; }

        public int Estado { get; set; }
        public string NombreEstado { get; set; }


        public string Observaciones { get; set; }

        public string RazonNoPlanificado { get; set; }
        public bool RequiereReemplazo { get; set; }

        public decimal VacacionesAcumuladas { get; set; }

        public List<SolicitudPlanificacionVacaciones> ListaPLanificacionVacaciones { get; set; }

        public int IdSolicitudPlanificacionVacaciones { get; set; }
    }
}
