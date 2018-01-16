using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public partial  class Calificacion
    {
        public Calificacion()
        {
            CandidatoConcurso = new HashSet<CandidatoConcurso>();
        }

        [Key]
        public int IdCalificacion { get; set; }
        public int IdTipoCalificacion { get; set; }
        public int IdCandidatoConcurso { get; set; }
        public int Puntaje { get; set; }

        public virtual ICollection<CandidatoConcurso> CandidatoConcurso { get; set; }
        public virtual TipoCalificacion TipoCalificacion { get; set; }
    }
}
