using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ActividadesEsencialesPorEval001ViewModel
    {
        public int IdEval001 { get; set; }
        
        public int IdEvaluacionActividadesPuestoTrabajo { get; set; }

        public int IdActividadesEsenciales { get; set; }
        public string NombreActividadEsencial { get; set; }
        
        public int IdIndicador { get; set; }
        public String NombreIndicador { get; set; }


        public int MetaPeriodo { get; set; }
        
        public int ActividadesCumplidas { get; set; }
        
        public double? PorcentajeCumplimiento { get; set; }

        public int? NivelCumplimiento { get; set; }

        public int Aumento { get; set; }
        

    }
}
