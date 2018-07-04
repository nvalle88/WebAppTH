using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class IndicesOcupacionalesModalidadPartidaViewModel
    {
        public int IdIndiceOcupacionalModalidadPartida { get; set; }
        public DateTime Fecha { get; set; }

        [Display(Name = "Remuneración")]
        public decimal? SalarioActual { get; set; }
        public int IdIndiceOcupacional { get; set; }
        public int? IdEmpleado { get; set; }
        public int? IdFondoFinanciamiento { get; set; }
        public int? IdTipoNombramiento { get; set; }

        [Display(Name = "Código de contrato")]
        public string CodigoContrato { get; set; }
        public int? IdModalidadPartida { get; set; }


        [Display(Name = "Partida individual")]
        public string NumeroPartidaIndividual { get; set; }

        [Display(Name = "Fecha fin")]
        public DateTime? FechaFin { get; set; }

        // Datos extra poner aquí debajo
        public IndiceOcupacionalViewModel IndiceOcupacionalViewModel { get; set; }

        [Display(Name = "Fondo de financiamiento")]
        public string NombreFondoFinanciamiento { get; set; }


        [Display(Name = "Tipo de nombramiento")]
        public string NombreTipoNombramiento { get; set; }
        public int? IdRelacionLaboral { get; set; }

        [Display(Name = "Relación laboral")]
        public string NombreRelacionLaboral { get; set; }


        public string NombreModalidadPartida { get; set; }

    }
}
