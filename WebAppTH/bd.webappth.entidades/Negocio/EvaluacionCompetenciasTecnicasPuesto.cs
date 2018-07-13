namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EvaluacionCompetenciasTecnicasPuesto
    {
        [Key]
        public int IdEvaluacionCompetenciasTecnicasPuesto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<Eval001> Eval001 { get; set; }

        public virtual ComportamientoObservable ComportamientoObservable { get; set; }
        
        public virtual NivelDesarrollo NivelDesarrollo { get; set; }
    }
}
