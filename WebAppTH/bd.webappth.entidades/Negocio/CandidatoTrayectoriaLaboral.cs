using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
    public class CandidatoTrayectoriaLaboral
    {
        public int IdCandidatoTrayectoriaLaboral { get; set; }
        public int IdCandidato { get; set; }
        [Display(Name = "Ingreso sector público:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicio { get; set; }
        [Display(Name = "Ingreso sector público:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFin { get; set; }
        public string Institucion { get; set; }

        public virtual Candidato Candidato { get; set; }
    }
}
