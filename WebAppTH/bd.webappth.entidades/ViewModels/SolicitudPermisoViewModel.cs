using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class SolicitudPermisoViewModel
    {
        [Display(Name = "Nombres y apellidos")]
        public string NombresApellidosEmpleado { get; set; }

        [Display(Name = "Dependencia")]
        public string NombreDependencia { get; set; }

        [Display(Name = "Solicitud")]
        public SolicitudPermiso SolicitudPermiso { get; set; }

        [Display(Name = "Tipo de permiso")]
        public string NombreTipoPermiso { get; set; }

        [Display(Name = "Estado")]
        public string NombreEstadoAprobacion { get; set; }
        public List<AprobacionMovimientoInternoViewModel> EstadoLista { get; set; }
    }

}
