using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelDocumentoIngresoEmpleado
    {
        public DistributivoHistorico Distributivo { get; set; }
        public List<DocumentosIngreso> ListaDocumentosIngreso { get; set; }
        public List<DocumentosIngresoEmpleado> ListaDocumentosIngresoEntregado { get; set; }
        public List<String> DocumentosSeleccionados { get; set; }
    }
}
