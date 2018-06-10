using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class DetalleNomina
    {
        [Key]
        public int IdDetalleNomina { get; set; }
        public int IdConceptoNomina { get; set; }
        public float Valor { get; set; }
        public int Signo { get; set; }

        public int IdCabeceraNomina { get; set; }
        public virtual CabeceraNomina CabeceraNomina { get; set; }
    }
}
