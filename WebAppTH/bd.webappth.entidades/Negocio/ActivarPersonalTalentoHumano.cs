using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class ActivarPersonalTalentoHumano
    {
        public int IdActivarPersonalTalentoHumano { get; set; }
        public int IdDependencia { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }

        public virtual Dependencia Dependencia { get; set; }
    }
}
