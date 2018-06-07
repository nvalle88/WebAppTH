namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EvaluacionConocimiento
    {
        /*
        [Key]
        public int IdEvaluacionConocimiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Conocimiento:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<Eval001> Eval001 { get; set; }

        public virtual ICollection<EvaluacionConocimientoFactor> EvaluacionConocimientoFactor { get; set; }

        public virtual ICollection<EvaluacionConocimientoDetalle> EvaluacionConocimientoDetalle { get; set; }
        */
        [Key]
        public int IdEvaluacionConocimiento { get; set; }
        public int? IdNivelConocimiento { get; set; }
        public int? IdEval001 { get; set; }
        public int? IdAreaConocimiento { get; set; }

        public virtual AreaConocimiento AreaConocimiento { get; set; }
        public virtual Eval001 Eval001 { get; set; }
        public virtual NivelConocimiento NivelConocimiento { get; set; }
    }
}
