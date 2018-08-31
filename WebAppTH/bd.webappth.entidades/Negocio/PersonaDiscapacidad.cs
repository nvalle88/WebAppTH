namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
     

    public partial class PersonaDiscapacidad
    {

        public int IdPersonaDiscapacidad { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Numero carnet CONADIS:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string NumeroCarnet { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Porcentaje de discapacidad:")]
        [Range(1,100, ErrorMessage = "El {0} no tiene que estar entre {1} y {2}")]
        public int? Porciento { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Tipo de discapacidad:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoDiscapacidad { get; set; }
        public virtual TipoDiscapacidad TipoDiscapacidad { get; set; }

        [Display(Name = "Persona:")]
        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPersona { get; set; }
        public virtual Persona Persona { get; set; }

        public virtual ICollection<PersonaSustituto> PersonaSustituto { get; set; }
    }
}
