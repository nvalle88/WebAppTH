using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class SolicitudTipoViatico
    {
        public int IdSolicitudTipoViatico { get; set; }
        public int IdTipoViatico { get; set; }
        public int IdSolicitudViatico { get; set; }

        public virtual SolicitudViatico SolicitudViatico { get; set; }
        public virtual TipoViatico TipoViatico { get; set; }
    }
}
