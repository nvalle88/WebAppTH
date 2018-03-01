using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class AgregarActividadEsencialViewModel
    {
        public List<ActividadesEsencialesViewModel> ListaActividadEsencial { get; set; }
        public List<String> DocumentosSeleccionados { get; set; }
    }
}
