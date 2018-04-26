using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class CrearActividadesGestionCambioViewModel
    {
        public ActividadesGestionCambioViewModel actividadesGestionCambioViewModel { get; set; }

        public List<DependenciaViewModel> ListaDependenciasViewModel { get; set; }
        public List<DatosBasicosEmpleadoViewModel> ListaDatosBasicosEmpleadoViewModel { get; set; }
        public List<EstadoActividadGestionCambioViewModel> ListaEstadoActividadGestionCambioViewModel { get; set; }
    }
}
