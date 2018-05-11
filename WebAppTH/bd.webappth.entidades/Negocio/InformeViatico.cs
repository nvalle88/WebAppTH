namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class InformeViatico
    {
        [Key]
        public int IdInformeViatico { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Itinerario de viático:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdItinerarioViatico { get; set; }
        public int IdSolicitudViatico { get; set; }
        public int? IdTipoTransporte { get; set; }
        [Display(Name = "Nombre Transporte:")]
        public string NombreTransporte { get; set; }
        public int? IdCiudadOrigen { get; set; }
        public int? IdCiudadDestino { get; set; }
        [Display(Name = "Fecha Llegada:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaLlegada { get; set; }
        [Display(Name = "Fecha Salida:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaSalida { get; set; }
        [Display(Name = "Hora Llegada:")]
        public TimeSpan? HoraLlegada { get; set; }
        [Display(Name = "Hora Salida:")]
        public TimeSpan? HoraSalida { get; set; }
        [Display(Name = "Valor:")]
        public decimal? ValorEstimado { get; set; }
        public virtual Ciudad CiudadDestino { get; set; }
        public virtual Ciudad CiudadOrigen { get; set; }
        public virtual TipoTransporte TipoTransporte { get; set; }
        public virtual ItinerarioViatico ItinerarioViatico { get; set; }
    }
}
