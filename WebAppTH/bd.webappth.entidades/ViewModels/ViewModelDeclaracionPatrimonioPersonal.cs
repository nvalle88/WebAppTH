using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelDeclaracionPatrimonioPersonal
    {
        public DeclaracionPatrimonioPersonal DeclaracionPatrimonioPersonalPasado { get; set; }
        public DeclaracionPatrimonioPersonal DeclaracionPatrimonioPersonalActual { get; set; }

        public OtroIngreso OtroIngresoActual { get; set; }

        public int IdEmpleado { get; set; }
    }
}
