using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ReliquidacionViaticoViewModel
    {
        public SolicitudViatico SolicitudViatico { get; set; }
        public List<SolicitudTipoViatico> SolicitudTipoViatico { get; set; }
        public List<ReliquidacionViatico> ReliquidacionViatico { get; set; }
        public ListaEmpleadoViewModel ListaEmpleadoViewModel { get; set; }
        public List<FacturaViatico> FacturaViatico { get; set; }
        public int IdItinerarioViatico { get; set; }
        public int IdSolicitudViatico { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorReliquidacion { get; set; }
        public decimal ValorTotalReliquidacion { get; set; }
        public decimal EstadoReliquidacion { get; set; }
        //public List<String> ViaticosSeleccionados { get; set; }
    }
}
