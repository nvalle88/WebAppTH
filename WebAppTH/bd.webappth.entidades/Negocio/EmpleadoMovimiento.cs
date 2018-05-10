namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class EmpleadoMovimiento
    {
        [Key]
        public int IdEmpleadoMovimiento { get; set; }
        public int IdEmpleado { get; set; }
        public int? IdIndiceOcupacionalModalidadPartidaDesde { get; set; }
        public int? IdIndiceOcupacionalModalidadPartidaHasta { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int IdAccionPersonal { get; set; }

        
    }
}
