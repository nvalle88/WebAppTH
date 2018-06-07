﻿using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class SolicitudViaticoViewModel
    {
        public SolicitudViatico SolicitudViatico { get; set; }
        public Presupuesto Presupuesto { get; set; }
        public List<SolicitudTipoViatico> SolicitudTipoViatico { get; set; }
        public List<ItinerarioViatico> ItinerarioViatico { get; set; }
        public ListaEmpleadoViewModel ListaEmpleadoViewModel { get; set; }
        public List<TipoViatico> ListaTipoViatico { get; set; }      
        public List<String>ViaticosSeleccionados { get; set; }
        public ReliquidacionViatico ReliquidacionViatico { get; set; }
        public Decimal Valor { get; set; }
        public int Reliquidacion { get; set; }

    }
}
