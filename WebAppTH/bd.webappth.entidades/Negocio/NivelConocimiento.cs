namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;



    public partial class NivelConocimiento
    {
        [Key]
        public int IdNivelConocimiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nivel de conocimiento:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<EvaluacionConocimiento> EvaluacionConocimiento { get; set; }
    }
}
