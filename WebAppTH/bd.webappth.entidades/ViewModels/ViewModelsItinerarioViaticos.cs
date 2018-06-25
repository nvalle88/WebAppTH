using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelsItinerarioViaticos
    {
        public int IdItinerarioViatico { get; set; }
        public int IdSolicitudViatico { get; set; }
        public int IdTipoTransporte { get; set; }
        [Display(Name = "Fecha desde:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDesde { get; set; }
        [Display(Name = "Fecha hasta:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaHasta { get; set; }
        [Display(Name = "Hora salida:")]
        public TimeSpan HoraSalida { get; set; }
        [Display(Name = "Hora llegada:")]
        public TimeSpan HoraLlegada { get; set; }
        public int IdCiudadOrigen { get; set; }
        public int IdCiudadDestino { get; set; }
        [Display(Name = "Ruta:")]
        public string Ruta { get; set; }
        [Display(Name = "Transporte:")]
        public string Transporte { get; set; }
    }
}
