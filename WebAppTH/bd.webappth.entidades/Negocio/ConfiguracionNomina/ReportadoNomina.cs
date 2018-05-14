using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class ReportadoNomina
    {
        [Key]
        public int IdReportadoNomina { get; set; }
        public string CodigoConcepto { get; set; }
        public string IdentificacionEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public double Cantidad { get; set; }
        public double Importe { get; set; }
        public bool Valido { get; set; }
        public string MensajeError { get; set; }

        public int IdCalculoNomina { get; set; }
        public virtual CalculoNomina CalculoNomina { get; set; }

       
    }
}
