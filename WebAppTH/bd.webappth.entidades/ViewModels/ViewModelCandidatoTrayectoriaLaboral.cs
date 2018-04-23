using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelCandidatoTrayectoriaLaboral
    {
        public int IdCandidatoTrayectoriaLaboral { get; set; }
        public int IdCandidato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Institucion { get; set; }
        public string ExperienciaDias { get; set; }
        public string ExperienciaMesAno { get; set; }

        public virtual Candidato Candidato { get; set; }
    }
}
