using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class FuncionNomina
    {
        [Key]
        public int IdFuncion { get; set; }
        public string Descripcion { get; set; }
        public string ProcedimientoAlmacenado { get; set; }
    }
}
