using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class RequerimientoRolPorGrupoOcupacionalViewModel
    {
        
        public int IdGrupoOcupacional { get; set; }
        public string NombreGrupoOcupacional { get; set; }

        public List<RequerimientoRolViewModel> ListaRolesRequeridos { get; set; }
    }
}
