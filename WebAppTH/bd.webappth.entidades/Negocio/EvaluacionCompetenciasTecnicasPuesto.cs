namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EvaluacionCompetenciasTecnicasPuesto
    {
        [Key]
        public int IdEvaluacionCompetenciasTecnicasPuesto { get; set; }
        public int? IdNivelDesarrollo { get; set; }
        public int? IdEval001 { get; set; }
        public int? IdComportamientoObservable { get; set; }

        public virtual ComportamientoObservable ComportamientoObservable { get; set; }
        public virtual Eval001 Eval001 { get; set; }
        public virtual NivelDesarrollo NivelDesarrollo { get; set; }
    }
}
