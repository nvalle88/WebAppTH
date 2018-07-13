namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DenominacionCompetencia
    {
        [Key]
        public int IdDenominacionCompetencia { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Definici�n")]
        [DataType(DataType.Text)]
        public string Definicion { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "�Es competencia t�cnica?")]
        public bool CompetenciaTecnica { get; set; }

        public virtual ICollection<ComportamientoObservable> ComportamientoObservable { get; set; }
    }
}
