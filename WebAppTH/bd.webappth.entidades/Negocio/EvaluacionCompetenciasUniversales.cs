namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EvaluacionCompetenciasUniversales
    {
        [Key]
        public int IdEvaluacionCompetenciasUniversales { get; set; }
        public int? IdFrecuenciaAplicacion { get; set; }
        public int? IdEval001 { get; set; }
        public int? IdComportamientoObservable { get; set; }

        public virtual ComportamientoObservable ComportamientoObservable { get; set; }
        public virtual Eval001 Eval001 { get; set; }
        public virtual FrecuenciaAplicacion FrecuenciaAplicacion { get; set; }
    }
}
