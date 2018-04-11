using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
  public class ContactoEmergenciaViewModel
    {
        public int IdPersona { get; set; }
        public int IdEmpleado { get; set; }
        public int IdEmpleadoContactoEmergencia { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Parentesco:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdParentesco { get; set; }
        public string Parentesco { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre:")]
        [StringLength(100, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Apellido:")]
        [StringLength(100, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono privado:")]
        [StringLength(11, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string TelefonoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono de casa:")]
        [StringLength(10, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string TelefonoCasa { get; set; }

    }
}
