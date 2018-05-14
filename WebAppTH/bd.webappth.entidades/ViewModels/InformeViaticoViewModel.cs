using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class InformeViaticoViewModel
    {
        public SolicitudViatico SolicitudViatico { get; set; }
        public List<SolicitudTipoViatico> SolicitudTipoViatico { get; set; }
        public List<InformeViatico> InformeViatico { get; set; }
        public ListaEmpleadoViewModel ListaEmpleadoViewModel { get; set; }
        public List<FacturaViatico> FacturaViatico { get; set; }
        public int IdItinerarioViatico { get; set; }
        public int IdSolicitudViatico { get; set; }
        public string Descripcion { get; set; }
        //public List<String> ViaticosSeleccionados { get; set; }
    }
}
