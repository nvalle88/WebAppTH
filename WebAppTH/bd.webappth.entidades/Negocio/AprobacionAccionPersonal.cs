using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class AprobacionAccionPersonal
    {
        public int IdAprobacionAccionPersonal { get; set; }
        public int IdAccionPersonal { get; set; }
        public int IdFlujoAprobacion { get; set; }
        public int IdEmpleadoAprobador { get; set; }
        public int Estado { get; set; }
        public DateTime FechaAprobacion { get; set; }

        public virtual AccionPersonal AccionPersonal { get; set; }
        public virtual Empleado EmpleadoAprobador { get; set; }
        public virtual FlujoAprobacion FlujoAprobacion { get; set; }
    }
}
