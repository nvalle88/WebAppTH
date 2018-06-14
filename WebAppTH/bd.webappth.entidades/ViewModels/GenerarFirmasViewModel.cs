using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class GenerarFirmasViewModel
    {
        public List<IdFiltrosViewModel>ListaIdEmpleados { get; set; }
        public int CantidadFirmas { get; set; }
        public String UrlReporte { get; set; }
    }
}
