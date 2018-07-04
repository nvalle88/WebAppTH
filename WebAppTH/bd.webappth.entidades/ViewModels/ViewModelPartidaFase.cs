using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelPartidaFase
    {
        public int? idescalagrados { get; set; }
        [Display(Name = "Puesto institucional:")]
        public string PuestoInstitucional { get; set; }
        [Display(Name = "Grupo ocupacional:")]
        public string grupoOcupacional { get; set; }
        public int Idindiceocupacional { get; set; }
        public int IdTipoConcurso { get; set; }
        public int IdPartidaFase { get; set; }
        [Display(Name = "Estado:")]
        public int estado { get; set; }
        public int Vacantes { get; set; }
        [Display(Name = "Vacantes creadas:")]
        public int VacantesCredo { get; set; }
        public bool Contrato { get; set; }
    }
}
