using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class SituacionActualEmpleadoViewModel
    {
        public int IdEmpleado { get; set; }

        public int IdSucursal { get; set; }

        [Display(Name = "Área")]
        public string NombreSucursal { get; set; }

        public int IdDependencia { get; set; }

        [Display(Name = "Unidad")]
        public string NombreDependencia { get; set; }

        public int IdCargo { get; set; }

        [Display(Name = "Cargo")]
        public string NombreCargo { get; set; }

        [Display(Name = "RMU")]
        public decimal Remuneracion { get; set; }

        public int IdIndiceOcupacionalModalidadPartida { get; set; }

    }
}
