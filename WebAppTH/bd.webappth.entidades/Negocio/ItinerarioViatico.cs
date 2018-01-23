namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ItinerarioViatico
    {
        [Key]

        public int IdItinerarioViatico { get; set; }
        public int IdSolicitudViatico { get; set; }
        public int IdTipoTransporte { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal Valor { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }

        public virtual ICollection<FacturaViatico> FacturaViatico { get; set; }
        public virtual ICollection<InformeViatico> InformeViatico { get; set; }
        public virtual SolicitudViatico SolicitudViatico { get; set; }
        public virtual TipoTransporte TipoTransporte { get; set; }



    }
}
