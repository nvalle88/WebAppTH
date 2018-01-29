using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class DocumentosIngreso
    {
      

        public int IdDocumentosIngreso { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<DocumentosIngresoEmpleado> DocumentosIngresoEmpleado { get; set; }
    }
}
