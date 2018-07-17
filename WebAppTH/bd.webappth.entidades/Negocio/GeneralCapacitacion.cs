using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class GeneralCapacitacion
    {
       

        public int IdGeneralCapacitacion { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacionAmbitoCapacitacion { get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacionEstadoEvento { get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacionNombreEvento { get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacionTipoCapacitacion{ get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacionTipoEvaluacion { get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacionTipoEvento { get; set; }
    }
}
