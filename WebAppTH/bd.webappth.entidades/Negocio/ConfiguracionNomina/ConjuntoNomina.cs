using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class ConjuntoNomina
    {
        public ConjuntoNomina()
        {
            ConceptoConjuntoNomina = new HashSet<ConceptoConjuntoNomina>();
        }

        public int IdConjunto { get; set; }
        public int IdTipoConjunto { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<ConceptoConjuntoNomina> ConceptoConjuntoNomina { get; set; }
        public virtual TipoConjuntoNomina IdTipoConjuntoNavigation { get; set; }
    }
}
