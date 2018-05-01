using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class AccionesPersonalPorEmpleadoViewModel
    {

        public string NombreUsuarioActual { get; set; }

        public List<AccionPersonalViewModel> ListaAccionPersonal { get; set; }

        public DatosBasicosEmpleadoViewModel DatosBasicosEmpleadoViewModel { get; set; }


    }
}
