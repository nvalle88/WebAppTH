using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DocumentoFAOViewModel
    {
        public int IdEmpleado { get; set; }
        [DisplayName("Nombres")]
        public string nombre { get; set; }
        [DisplayName("Apellido")]
        public string apellido { get; set; }
        [DisplayName("Identificacion")]
        public string Identificacion { get; set; }
        public int OpcionMenu { get; set; }
        public string NombreUsuario { get; set; }
        public int idDependencia { get; set; }
        public int idsucursal { get; set; }
        public int ? estado { get; set; }

    }
}
