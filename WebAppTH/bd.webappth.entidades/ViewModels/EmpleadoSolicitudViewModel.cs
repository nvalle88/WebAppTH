using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class EmpleadoSolicitudViewModel 
    {
        public string NombreApellido { get; set; }
        public string Identificacion { get; set; }
        [Display(Name = "Revisado")]
        public bool Aprobado { get; set; }

        [Display(Name = "Solicitudes nuevas?")]
        public bool HaSolicitado { get; set; }

        public int IdEmpleado { get; set; }
    }
}
