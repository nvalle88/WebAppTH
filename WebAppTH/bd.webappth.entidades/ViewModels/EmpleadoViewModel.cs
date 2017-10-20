using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class EmpleadoViewModel
    {
        public Persona Persona { get; set; }
        public Empleado Empleado { get; set; }
        public IndiceOcupacionalModalidadPartida IndiceOcupacionalModalidadPartida { get; set; }
    }
}
