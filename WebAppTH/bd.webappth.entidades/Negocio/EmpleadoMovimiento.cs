namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class EmpleadoMovimiento
    {
        public int IdEmpleadoMovimiento { get; set; }
        public int IdEmpleado { get; set; }
        public int? IdIndiceOcupacionalModalidadPartidaDesde { get; set; }
        public int? IdIndiceOcupacionalModalidadPartidaHasta { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int IdAccionPersonal { get; set; }
        public int? IdIndiceOcupacional { get; set; }
        public int? IdFondoFinanciamiento { get; set; }
        public int? IdTipoNombramiento { get; set; }
        public decimal? SalarioReal { get; set; }
        public string CodigoContrato { get; set; }
        public string NumeroPartidaIndividual { get; set; }
        public int? IdModalidadPartida { get; set; }
        public bool EsJefe { get; set; }

        public virtual AccionPersonal AccionPersonal { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual FondoFinanciamiento FondoFinanciamiento { get; set; }
        public virtual IndiceOcupacional IndiceOcupacional { get; set; }
        public virtual ModalidadPartida ModalidadPartida { get; set; }
        public virtual TipoNombramiento TipoNombramiento { get; set; }
        public virtual IndiceOcupacionalModalidadPartida IndiceOcupacionalModalidadPartidaDesde { get; set; }
        public virtual IndiceOcupacionalModalidadPartida IndiceOcupacionalModalidadPartidaHasta { get; set; }


    }
}
