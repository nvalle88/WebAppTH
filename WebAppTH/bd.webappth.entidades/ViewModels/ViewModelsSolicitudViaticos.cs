using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelsSolicitudViaticos
    {
        public int IdSolicitudViatico { get; set; }
        public int IdItinerario { get; set; }
        public int IdPresupuesto { get; set; }        
        public int IdEmpleado { get; set; }
        [Display(Name = "Servidor:")]
        public string Servidor { get; set; }
        public int IdCiudad { get; set; }
        [Display(Name = "Ciudad:")]
        public string Ciudad { get; set; }
        [Display(Name = "Provincia:")]
        public string Provincia { get; set; }
        [Display(Name = "País:")]
        public string Pais { get; set; }
        public int IdConfiguracionViatico { get; set; }
        [Display(Name = "Fecha solicitud:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSolicitud { get; set; }
        [Display(Name = "Fecha solicitud:")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha solicitud:")]
        public decimal ValorEstimado { get; set; }
        [Display(Name = "Fecha llegada:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaLlegada { get; set; }
        [Display(Name = "Fecha salida:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }
        [Display(Name = "Observación:")]
        public string Observacion { get; set; }
        public int Estado { get; set; }
        [Display(Name = "Hora salida:")]
        public TimeSpan HoraSalida { get; set; }
        [Display(Name = "Hora llegada:")]
        public TimeSpan HoraLlegada { get; set; }
        public int? IdEmpleadoAprobador { get; set; }
        [Display(Name = "Fondo financiamiento:")]
        public string FondoFinanciamiento { get; set; }
        public int IdFondoFinanciamiento { get; set; }
        [Display(Name = "Dependencia:")]
        public string Dependencia { get; set; }
        [Display(Name = "Puesto:")]
        public string Puesto { get; set; }
        public int Reliquidacion { get; set; }
        public decimal Valor { get; set; }

        public List<TipoViatico> ListaTipoViatico { get; set; }
        public List<ViewModelsItinerarioViaticos> ListaItinerarioViatico { get; set; }
        public List<InformeViatico> ListaInformeViatico { get; set; }
        public InformeActividadViatico InformeActividadViatico { get; set; }
        public List<FacturaViatico> ListaFacturaViatico { get; set; }
        public List<ReliquidacionViatico> ListaReliquidacionViatico { get; set; }
    }
}
