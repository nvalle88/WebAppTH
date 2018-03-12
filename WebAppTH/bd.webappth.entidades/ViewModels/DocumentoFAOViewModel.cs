using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DisplayName("Número de Cédula")]
        public string Identificacion { get; set; }
        public int OpcionMenu { get; set; }
        public string NombreUsuario { get; set; }
        public int idDependencia { get; set; }
        public int idsucursal { get; set; }
        public int ? estado { get; set; }
        [DisplayName("Institución")]
        public string Institucion { get; set; }
        [DisplayName("Unidad Administrativa")]
        public string UnidadAdministrativa { get; set; }
        [DisplayName("Puesto")]
        public string Puesto { get; set; }
        [DisplayName("Nombre Apellido")]
        public string NombreApellido { get; set; }
        [DisplayName("Lugar Trabajo")]
        public string LugarTrabajo { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [DisplayName("Mision")]
        public string Mision { get; set; }
        [DisplayName("Actividad")]
        public string actividad{get; set; }

    }
}
