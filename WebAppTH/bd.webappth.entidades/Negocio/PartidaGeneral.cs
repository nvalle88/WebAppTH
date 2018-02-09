
using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class PartidaGeneral
    {
      
        public int IdPartidaGeneral { get; set; }
        public string NumeroPartida { get; set; }

        public virtual ICollection<IndiceOcupacional> IndiceOcupacional { get; set; }
    }
}
