namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class PartidasFase
    {
        [Key]
        public int IdPartidasFase { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        //[Display(Name = "¿Ganador?")]
        //public bool? Ganador { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Indice Ocupacional Modalidad Partida:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdIndiceOcupacional { get; set; }
        public virtual IndiceOcupacional IndiceOcupacional{ get; set; }

        [Display(Name = "Fase del concurso:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoConcurso { get; set; }
        public DateTime? Fecha { get; set; }
        public int Vacantes { get; set; }
        public int Estado { get; set; }
        public bool Contrato { get; set; }
        public virtual ICollection<CandidatoConcurso> CandidatoConcurso { get; set; }
        public virtual TipoConcurso TipoConcurso { get; set; }


    }
}
