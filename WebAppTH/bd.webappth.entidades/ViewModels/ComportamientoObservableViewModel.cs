using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class ComportamientoObservableViewModel
    {
        public int IdComportamientoObservable { get; set; }

        public int IdIndiceOcupacional { get; set; }

        public string Descripcion { get; set; }

        public int IdNivel { get; set; }

        public string NombreNivel { get; set; }

        public int IdDenominacionCompetencia { get; set; }

        public string NombreDenominacionCompetencia { get; set; }

        public string DefinicionDenominacionCompetencia { get; set; }

        public bool CompetenciaTecnicaDenominacionCompetencia { get; set; }
    }
}
