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
        [Display(Name = "Código del concepto")]
        public string CodigoConcepto { get; set; }
        [Display(Name = "Identificación del empleado")]
        public string IdentificacionEmpleado { get; set; }
        [Display(Name = "Nombre del empleado")]
        public string NombreEmpleado { get; set; }
        [Display(Name = "Cantidad")]
        public double Cantidad { get; set; }
        [Display(Name = "Importe")]
        public double Importe { get; set; }
        public bool Valido { get; set; }
        [Display(Name = "Descripción del error")]
        public string MensajeError { get; set; }

        public int IdCalculoNomina { get; set; }
        public virtual CalculoNomina CalculoNomina { get; set; }

       
    }
}
