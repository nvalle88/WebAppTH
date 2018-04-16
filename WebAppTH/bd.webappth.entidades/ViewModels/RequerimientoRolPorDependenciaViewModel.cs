using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class RequerimientoRolPorDependenciaViewModel
    {
        public string NombreUsuario { get; set; }

        public int IdDependencia { get; set; }
        public string NombreDependencia { get; set; }
        
        public RequerimientoRolPorGrupoOcupacionalViewModel RolesNivelJerarquicoSuperior { get; set; }
        public RequerimientoRolPorGrupoOcupacionalViewModel RolesNivelOperativo { get; set; }
    }
}
