using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class Induccion
    {
        public int IdInduccion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
    }
}
