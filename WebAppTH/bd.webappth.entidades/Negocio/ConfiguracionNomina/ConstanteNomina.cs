using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{ 
   public class ConstanteNomina
    {
        public int IdConstante { get; set; }
        public string Constante { get; set; }
        public double Valor { get; set; }
        public string Descripcion { get; set; }
    }
}
