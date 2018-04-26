using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.ViewModels
{
    public partial class ActividadesGestionCambioViewModel 
    {
        public string NombreUsuario { get; set; }

        public int IdActividadesGestionCambio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        [Display(Name = "Área responsable")]
        public int IdDependencia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        [Display(Name = "Responsable")]
        public int IdEmpleado { get; set; }
        
        [Display(Name = "Área responsable")]
        public string NombreDependencia { get; set; }
        
        [Display(Name = "Responsable")]
        public string NombreEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        public string Tarea { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de finalización")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        public int Avance { get; set; }

        public string Observaciones { get; set; }


        public int ValorEstado { get; set; }
        public string Estado { get; set; }

        
    }
}
