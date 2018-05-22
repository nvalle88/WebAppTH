
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DependenciaDatosViewModel
    {
        public int IdDependencia { get; set; }

        [Display(Name = "Dependencia")]
        public string NombreDependencia { get; set; }

        public int IdSucursal { get; set; }

        [Display(Name = "Sucursal")]
        public string NombreSucursal { get; set; }

        public DatosBasicosEmpleadoViewModel DatosBasicosEmpleadoJefeViewModel { get; set; }

        public List<DependenciaDatosViewModel> ListaDependenciasHijas{ get; set; }

        public List<DatosBasicosEmpleadoViewModel> ListaEmpleadosDependencia { get; set; }
    }
}
