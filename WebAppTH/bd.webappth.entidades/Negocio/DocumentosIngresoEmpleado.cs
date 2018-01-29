using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class DocumentosIngresoEmpleado
    {
        public int IdDocumentosIngresoEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public int IdDocumentosIngreso { get; set; }
        public bool? Entregado { get; set; }

        public virtual DocumentosIngreso DocumentosIngreso { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
