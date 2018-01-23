using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class TipoCesacionFuncion
    {
        public int IdTipoCesacionFuncion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<CeseFuncion> CeseFuncion { get; set; }
    }
}
