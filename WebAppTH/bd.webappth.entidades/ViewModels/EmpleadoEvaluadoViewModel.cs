using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class EmpleadoEvaluadoViewModel
    {
        
        public int IdEmpleado { get; set; }

        [Display(Name = "Nombres y apellidos")]
        public string NombresApellidos { get; set; }

        public int IdPuesto { get; set; }

        [Display(Name = "Puesto")]
        public string NombrePuesto { get; set; }

        [Display(Name = "Títulos")]
        public List<TituloViewModel> ListaTituloProfesion { get; set; }

    }
}
