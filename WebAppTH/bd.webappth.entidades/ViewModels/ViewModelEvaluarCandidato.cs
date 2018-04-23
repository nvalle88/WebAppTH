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
        public int IdPartidaFase { get; set; }
        //Creacion Postulacion
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public int? PorInstruccion { get; set; }
        public bool? ApliInstruccion { get; set; }
        public bool? ApliExperiencia { get; set; }
        public int? PorExperiencia { get; set; }
        public bool? Elegido { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int Total { get; set; }
        //listas
        public List<ViewModelEstudioCandidato> ListasPersonaEstudio { get; set; }
        public List<ViewModelCandidatoTrayectoriaLaboral> ListasCanditadoExperiencia { get; set; }
    }
   
}
