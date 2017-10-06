using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class IndiceOcupacionalExperienciaLaboralRequeridaView
    {
        public int IdIndiceOcupacional { get; set; }
        public List<ExperienciaLaboralRequerida> ListaExperienciaLaboralRequerida { get; set; }
       
    }
}
