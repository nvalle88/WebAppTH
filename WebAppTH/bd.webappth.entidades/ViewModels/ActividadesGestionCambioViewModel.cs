using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ActividadesGestionCambioViewModel
    {
        public int IdPlanGestionCambio { get; set; }
        public List<ActividadesGestionCambioIndex> ListaActividadesGestionCambio { get; set; }
    }
}
