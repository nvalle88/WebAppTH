using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class FormulaNomina
    {
        public string Formula { get; set; }
        public int IdRegimenLaboral { get; set; }
        public int IdConceptoNomina { get; set; }

        public virtual ConceptoNomina ConceptoNomina { get; set; }
        public virtual RegimenLaboral RegimenLaboral { get; set; }
    }
}
