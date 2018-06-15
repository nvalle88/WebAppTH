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

        [Display(Name = "Rol")]
        public string NombreRolPuesto { get; set; }

        public int IdManualPuesto { get; set; }

        [Display(Name = "Cargo")]
        public string NombreManualPuesto { get; set; }

        public int IdModalidadPartida { get; set; }

        [Display(Name = "Modalidad")]
        public string NombreModalidadPartida { get; set; }
        
        [Display(Name = "Grupo ocupacional")]
        public string GrupoOcupacional { get; set; }

        [Display(Name = "RMU")]
        public decimal? RMU { get; set; }
        
        public int Grado { get; set; }

        [Display(Name = "Cantidad de personas")]
        public int CantidadEmpleados { get; set; }

        [Display(Name = "Partida Individual")]
        public string PartidaIndividual { get; set; }

    }
}
