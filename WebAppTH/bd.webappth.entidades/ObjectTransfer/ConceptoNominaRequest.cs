using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ObjectTransfer
{
   public class ConceptoNominaRequest
    {
        public string CodigoConcepto { get; set; }
        public int IdCalculoNomina { get; set; }
        public List<ReportadoNomina> ListaExcel { get; set; }
    }
}
