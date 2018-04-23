using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelEvaluarCandidato
    {

        public int IdCandidato { get; set; }
        //Creacion Postulacion
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        //listas
        public List<ViewModelEstudioCandidato> ListasPersonaEstudio { get; set; }
        public List<CandidatoTrayectoriaLaboral> ListasCanditadoExperiencia { get; set; }
    }
   
}
