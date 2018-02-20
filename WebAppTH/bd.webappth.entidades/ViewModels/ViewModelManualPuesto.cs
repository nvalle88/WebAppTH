using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelManualPuesto
    {
        public ManualPuesto ManualPuesto { get; set; }
        public List<RelacionesInternasExternas> RelacionesInternasExternas { get; set; }
    }
}
