
namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    public partial class EvaluacionEvento
    {        

        public int IdEvaluacionEvento { get; set; }
        public int? IdPlanCapacitacion { get; set; }
        public string Sugerencias { get; set; }

        public virtual ICollection<DetalleEvaluacionEvento> DetalleEvaluacionEvento { get; set; }
        public virtual PlanCapacitacion PlanCapacitacion { get; set; }
    }
}
