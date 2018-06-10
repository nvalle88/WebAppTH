using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class CabeceraNomina
    {
        [Key]
        public int IdCabeceraNomina { get; set; }
        public int IdEmpleado { get; set; }
        public string Identificacion { get; set; }

        public int IdCalculoNomina { get; set; }
        public virtual CalculoNomina CalculoNomina { get; set; }

        public virtual ICollection<DetalleNomina> DetalleNomina { get; set; }
    }
}
