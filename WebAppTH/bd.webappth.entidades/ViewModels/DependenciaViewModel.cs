using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DependenciaViewModel
    {
        public int IdDependencia { get; set; }
        public string NombreDependencia { get; set; }
        public string NombreSucursal { get; set; }
        public string NombreDependenciaPadre { get; set; }
        public string NombreProceso { get; set; }
    }
}
