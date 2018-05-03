using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class TeconceptoNomina
    {
        public int IdTeconcepto { get; set; }
        public int IdConcepto { get; set; }

        public virtual ConceptoNomina ConceptoNomina { get; set; }
    }
}
