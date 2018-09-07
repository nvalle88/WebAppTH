namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EmpleadoMovimiento
    {
        public int IdEmpleadoMovimiento { get; set; }
        public int IdEmpleado { get; set; }
        public int IdIompDesde { get; set; }
        public int IdIompHasta { get; set; }
        public int IdAccionPersonal { get; set; }
        public int? IdFondoFinanciamiento { get; set; }
        public int? IdTipoNombramiento { get; set; }

        public virtual ICollection<DistributivoSituacionActual> DistributivoSituacionActual { get; set; }
        public virtual AccionPersonal AccionPersonal { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual IndiceOcupacionalModalidadPartida IompDesde { get; set; }
        public virtual IndiceOcupacionalModalidadPartida IompHasta { get; set; }


    }
}
