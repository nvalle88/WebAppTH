using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DependenciaViewModel
    {
        public int IdDependencia { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Sucursal")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdSucursal { get; set; }
        [Display(Name = "Orden ")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Debe introducir el {0}")]
        [Display(Name = "Proceso")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdProceso { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Ciudad")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdCiudad { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int? IdDependenciaPadre { get; set; }

        [Required(ErrorMessage = "Debe introducir la {0}")]
        [Display(Name = "Áreas Usuarias / Unidad administrativa")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La {0} no puede tener más de {1} y menos de {2}")]
        public string NombreDependencia { get; set; }

        [Display(Name = "Sucursal")]
        public string NombreSucursal { get; set; }
        [Display(Name = "Área padre ")]
        public string NombreDependenciaPadre { get; set; }
        [Display(Name = "Proceso")]
        public string NombreProceso { get; set; }
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }
    }
}
