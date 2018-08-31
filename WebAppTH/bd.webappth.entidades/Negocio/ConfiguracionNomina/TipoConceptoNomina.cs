using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class TipoConceptoNomina
    {
        public TipoConceptoNomina()
        {
            ConceptoNomina = new HashSet<ConceptoNomina>();
        }

        public int IdTipoConcepto { get; set; }
        public int Signo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<ConceptoNomina> ConceptoNomina { get; set; }
    }
}
