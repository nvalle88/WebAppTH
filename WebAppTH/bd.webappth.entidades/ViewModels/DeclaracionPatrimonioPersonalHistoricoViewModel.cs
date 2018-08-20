using System;
using System.Collections.Generic;
using System.Text;
using bd.webappth.entidades.Negocio;

namespace bd.webappth.entidades.ViewModels
{
    public class DeclaracionPatrimonioPersonalHistoricoViewModel
    {
        public int IdEmpleado { get; set; }

        public string NombrePersona { get; set; }
        public string ApellidoPersona { get; set; }

        public List<DeclaracionPatrimonioPersonal> ListaDeclaracionPatrimonioPersonal { get; set; }
    }
}
