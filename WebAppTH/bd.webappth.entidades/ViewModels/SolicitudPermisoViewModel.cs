using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class SolicitudPermisoViewModel
    {
        public string NombresApellidosEmpleado { get; set; }
        public string NombreDependencia { get; set; }
        public SolicitudPermiso SolicitudPermiso { get; set; }
        public int IdTipoPermiso { get; set; }
        public int Estado { get; set; }
        public List<ListaEstado> EstadoLista { get; set; }
    }

}
