using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
  public  class IndiceOcupacionalViewModel
    {
        // Campos del índice ocupacional

        public int IdIndiceOcupacional { get; set; }
        public int? IdDependencia { get; set; }
        public int? IdManualPuesto { get; set; }
        public int? IdRolPuesto { get; set; }
        public int? IdEscalaGrados { get; set; }
        public int? IdPartidaGeneral { get; set; }
        public int? IdAmbito { get; set; }
        public string Nivel { get; set; }

        // Campos extra agregar aquí debajo

        [Display(Name = "Unidad administrativa")]
        public string NombreDependencia { get; set; }

        [Display(Name = "Código unidad administrativa")]
        public string CodigoDependencia { get; set; }


        public int IdSucursal { get; set; }

        [Display(Name = "Sucursal")]
        public string NombreSucursal { get; set; }

        [Display(Name = "Denominación del puesto")]
        public string NombreManualPuesto { get; set; }

        [Display(Name = "Descripción del puesto")]
        public string DescripcionManualPuesto { get; set; }

        [Display(Name = "Misión del puesto")]
        public string MisionManualPuesto { get; set; }


        public int? IdRelacionesInternasExternas { get; set; }
        public string NombreRelacionesInternasExternas { get; set; }
        public string DescripcionRelacionesInternasExternas { get; set; }


        [Display(Name = "Rol")]
        public string NombreRolPuesto { get; set; }

        [Display(Name = "Grupo ocupacional")]
        public string NombreEscalaGrados { get; set; }

        [Display(Name = "Remuneración")]
        public decimal? Remuneracion { get; set; }
        public int? Grado { get; set; }


        [Display(Name = "Partida general")]
        public string NumeroPartidaGeneral { get; set; }


        [Display(Name = "Ámbito")]
        public string NombreAmbito { get; set; }

        public int OpcionMenu { get; set; }
    }
}
