using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class DiscapacidadSustitutoRequest
    {

        public int OpcionMenu { get; set; }
        public int IdPersonaSustituto { get; set; }
        public int IdDiscapacidadSustituto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de discapacidad:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoDiscapacidad { get; set; }

        public string NombreTipoDiscapacidad { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Porciento de discapacidad")]
        public int PorcentajeDiscapacidad{ get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número de carnet")]
        [StringLength(50, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string NumeroCarnet { get; set; }

        public List<ViewModelDiscapacidadSustituto> ListaDiscapacidadSustitutos { get; set; }
    }

    public class ViewModelDiscapacidadSustituto
    {

        public int IdDiscapacidadSustituto { get; set; }

        public int IdTipoDiscapacidad { get; set; }

        public string NombreTipoDiscapacidad { get; set; }

        public string NumeroCarnet { get; set; }

        public int PorcentajeDiscapacidad { get; set; }



    }
}
