using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class DistributivoHistorico
    {
        public int IdDistributivoHistorico { get; set; }
        public int IdIndiceOcupacionalModalidadPartida { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int IdFondoFinanciamiento { get; set; }
        public int IdTipoNombramiento { get; set; }
        public bool Activo { get; set; }

        public virtual Empleado Empleado { get; set; }
        public virtual FondoFinanciamiento FondoFinanciamiento { get; set; }
        public virtual IndiceOcupacionalModalidadPartida IndiceOcupacionalModalidadPartida { get; set; }
        public virtual TipoNombramiento TipoNombramiento { get; set; }
    }
}
