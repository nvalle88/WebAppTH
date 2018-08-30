using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelPersonaEstudio
    {
        [Key]
        public int IdPersonaEstudio { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de graduación:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaGraduado { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Observaciones:")]
        [DataType(DataType.MultilineText)]
        //[StringLength(500, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "NoSenescyt:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string NoSenescyt { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Area Conocimiento:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdAreaConocimiento { get; set; }

        [Display(Name = "Estudio:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEstudio { get; set; }

        [Display(Name = "Título:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTitulo { get; set; }

        [Display(Name = "Persona:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPersona { get; set; }

        [Display(Name = "Institución:")]
        public string Institucion { get; set; }
    }
}
