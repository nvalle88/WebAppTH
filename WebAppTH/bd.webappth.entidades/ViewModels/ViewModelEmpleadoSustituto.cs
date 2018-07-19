using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class ViewModelEmpleadoSustituto
    {


        //Datos de la persona sustituto

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre:")]
        [StringLength(100, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Apellido:")]
        [StringLength(100, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string Apellido { get; set; }

        public string NombreParentesco { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono privado:")]
        //[StringLength(11, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string TelefonoPrivado { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono de casa:")]
        //[StringLength(10, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string TelefonoCasa { get; set; }


        //Pesona Sustituto
        public int IdEmpleado { get; set; }
        public int IdPersonaSustituto { get; set; }
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Parentesco:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdParentesco { get; set; }


        //Enfermedad Sustituto

      

        //Discapacidas sustituto
        public int IdDiscapacidadSustituto { get; set; }

        
        public int IdTipoDiscapacidad { get; set; }

        public int PorcentajeDiscapacidad { get; set; }
        public string NumeroCarnet { get; set; }
        public int IdPersonaSustitutoDiscapacidad { get; set; }


        [NotMapped]
        public string Enfermedades { get; set; }

        [NotMapped]
        public string Discapacidades { get; set; }

    }
}
