using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class ExperienciaLaboralRequeridaViewModel
    {
        public int IdExperienciaLaboralRequerida { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public int IdEspecificidadExperiencia { get; set; }
        public string DescripcionEspecificidadExperiencia { get; set; }
        public int IdIndiceOcupacional { get; set; }

        public int IdEstudio { get; set; }
        public string NombreEstudio { get; set; }

        public int AnoExperiencia { get; set; }
        public int MesExperiencia { get; set; }
    }
}
