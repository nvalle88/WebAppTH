using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{

    public class EnfermedadSustitutoRequest
    {

        public int OpcionMenu { get; set; }
        public int IdPersonaSustituto { get; set; }
        public int IdEnfermedadSustituto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de enfermedad:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoEnfermedad { get; set; }

        public string NombreTipoEnfermedad { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Institución que emite")]
        [StringLength(100, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string InstitucionEmite { get; set; }

        public List<ViewModelEnfermedadSustituto> ListaEnfermedadesSustitutos { get; set; }
    }


   public class ViewModelEnfermedadSustituto
    {

        public int IdEnfermedadSustituto { get; set; }

        public int IdTipoEnfermedad { get; set; }

        public string NombreTipoEnfermedad { get; set; }

        public string InstitucionEmite { get; set; }


       
    }
}
