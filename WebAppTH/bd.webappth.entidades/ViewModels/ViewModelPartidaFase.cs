using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelPartidaFase
    {
        public int? idescalagrados { get; set; }
        public string PuestoInstitucional { get; set; }
        public string grupoOcupacional { get; set; }
        public int Idindiceocupacional { get; set; }
        public int IdTipoConcurso { get; set; }
        public int IdPartidaFase { get; set; }
        public int estado { get; set; }
        public int Vacantes { get; set; }
        public int VacantesCredo { get; set; }
    }
}
