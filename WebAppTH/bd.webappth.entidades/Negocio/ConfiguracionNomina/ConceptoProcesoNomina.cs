using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class ConceptoProcesoNomina
    {
        public int IdProcesoNomina { get; set; }
        public int IdConceptoNomina { get; set; }

        public virtual ConceptoNomina ConceptoNomina { get; set; }
        public virtual ProcesoNomina ProcesoNomina { get; set; }
    }
}
