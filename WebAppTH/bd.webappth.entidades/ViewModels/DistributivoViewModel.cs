using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DistributivoViewModel
    {
        public int IdDependencia { get; set; }

        [Display(Name = "Unidad administrativa")]
        public string NombreDependencia { get; set; }

        public int IdRolPuesto { get; set; }

        [Display(Name = "Puesto - cargo")]
        public string NombreRolPuesto { get; set; }

        public int IdModalidadPartida { get; set; }

        [Display(Name = "Modalidad")]
        public string NombreModalidadPartida { get; set; }
        [Display(Name = "Grupo ocupacional")]
        public string GrupoOcupacional { get; set; }

    }
}
