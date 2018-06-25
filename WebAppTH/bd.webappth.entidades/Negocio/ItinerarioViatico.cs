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
        [Display(Name = "Fecha Desde:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDesde { get; set; }
        [Display(Name = "Fecha Hasta:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaHasta { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }
        public int IdCiudadOrigen { get; set; }
        public int IdCiudadDestino { get; set; }

        public virtual ICollection<InformeViatico> InformeViatico { get; set; }
        public virtual ICollection<ReliquidacionViatico> ReliquidacionViatico { get; set; }
        public virtual Ciudad CiudadDestino { get; set; }
        public virtual Ciudad CiudadOrigen { get; set; }
        public virtual SolicitudViatico SolicitudViatico { get; set; }
        public virtual TipoTransporte TipoTransporte { get; set; }

        



    }
}
