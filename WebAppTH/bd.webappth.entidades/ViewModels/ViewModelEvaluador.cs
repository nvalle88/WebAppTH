using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelEvaluador
    {
        public int IdEmpleado { get; set; }
        [Display(Name = "Apellido y Nombres:")]
        public string NombreApellido { get; set; }
        [Display(Name = "Puesto")]
        public string Puesto { get; set; }
        [Display(Name = "Titulo o Profesión:")]
        public string Titulo { get; set; }
        [Display(Name = "Apellido y Nombres Jefe Inmediato:")]
        public string DatosJefe { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string NombreUsuario { get; set; }
        public List<ActividadesEsenciales> ListaActividad { get; set; }

    }
}
