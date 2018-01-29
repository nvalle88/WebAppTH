using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelDocumentoIngresoEmpleado
    {
        public ListaEmpleadoViewModel empleadoViewModel { get; set; }
        public List<DocumentosIngreso> listadocumentosingreso { get; set; }
        public List<DocumentosIngresoEmpleado> listadocumentosingresoentregado { get; set; }
        public List<String> DocumentosSeleccionados { get; set; }
    }
}
