namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class FrecuenciaAplicacion
    {
        [Key]
        public int IdFrecuenciaAplicacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Frecuencia de la Aplicaci�n:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases
        public virtual ICollection<EvaluacionCompetenciasUniversales> EvaluacionCompetenciasUniversales { get; set; }
        public virtual ICollection<EvaluacionTrabajoEquipoIniciativaLiderazgo> EvaluacionTrabajoEquipoIniciativaLiderazgo { get; set; }
    }
}
