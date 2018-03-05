using System.Collections.Generic;
using bd.webappth.entidades.Negocio;

namespace bd.webappth.entidades.ViewModels
{
    public class FichaMedicaViewModel
    {

        public DatosBasicosPersonaViewModel DatosBasicosPersonaViewModel { get; set; }
        public List<FichaMedica> FichasMedicas { get; set; }
        
    }
}
