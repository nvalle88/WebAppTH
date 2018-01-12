using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    class EmpleadoSolicitudHorasExtraViewModel
    {
        public string NombreApellido { get; set; }
        public string Identificacion { get; set; }
        [Display(Name = "Revisado")]
        public bool Aprobado { get; set; }
        public bool HaSolicitadoHorasExtra { get; set; }
        public int IdEmpleado { get; set; }
    }
}
