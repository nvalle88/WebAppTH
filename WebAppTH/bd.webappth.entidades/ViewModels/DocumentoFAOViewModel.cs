using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DocumentoFAOViewModel
    {
        public int IdEmpleado { get; set; }
        [DisplayName("Nombres Apellido")]
        public string nombreApellido { get; set; }
        [DisplayName("Identificacion")]
        public string Identificacion { get; set; }
        public int OpcionMenu { get; set; }
       
    }
}
