using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.Utils
{
    public class EmpleadoFAO
    {
        public List<ActividadesAnalisisOcupacional> ListaActividad { get; set; }
        public DocumentoFAOViewModel Empleado { get; set; }

    }
}
