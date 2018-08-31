using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class ProcesoNomina
    {
        public ProcesoNomina()
        {
            CalculoNomina = new HashSet<CalculoNomina>();
            ConceptoProcesoNomina = new HashSet<ConceptoProcesoNomina>();
        }

        public int IdProceso { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<CalculoNomina> CalculoNomina { get; set; }
        public virtual ICollection<ConceptoProcesoNomina> ConceptoProcesoNomina { get; set; }
    }
}
