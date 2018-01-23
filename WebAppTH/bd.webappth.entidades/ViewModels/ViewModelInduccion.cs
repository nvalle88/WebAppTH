using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ObjectTransfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelInduccion
    {
        public List<MaterialInduccion> Imagenes { get; set; }
        public List<MaterialInduccion> Archivos { get; set; }
        public List<MaterialInduccion> Videos { get; set; }
    }
}
