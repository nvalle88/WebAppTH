using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelSeleccionPersonal
    {
        public int? iddependecia { get; set; }
        public int IdCandidato { get; set; }
        public int IdPartidaFase { get; set; }
        public int NoSenescyt { get; set; }
        public DateTime FechaGraduado { get; set; }
        public string Observaciones { get; set; }
        [Display(Name = "Partida General")]
        public string NumeroPartidaGeneral { get; set; }
        [Display(Name = "Unidad Administrativa")]
        public string UnidadAdministrativa { get; set; }
        [Display(Name = "Partida Individual")]
        public string NumeroPartidaIndividual { get; set; }
        [Display(Name = "Puesto Institucional")]
        public string PuestoInstitucional { get; set; }
        [Display(Name = "Grupo Ocupacional")]
        public string grupoOcupacional { get; set; }
        [Display(Name = "Vacantes")]
        public int NumeroPuesto { get; set; }
        [Display(Name = "Rol")]
        public string Rol { get; set; }
        [Display(Name = "Remuneración")]
        public decimal? Remuneracion { get; set; }
        public int OpcionMenu { get; set; }
        public int IdCandidatoConcurso { get; set; }



        //Creacion Postulacion
        [Required]
        [Display(Name = "Identificación")]
        public string identificacion { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string nombres { get; set; }
        [Required]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }
        [Display(Name = "Nivel Instrucción")]
        public int IdEstudio { get; set; }
        public string nivelIntruccion { get; set; }
        [Display(Name = "Área Conocimiento")]
        public int IdAreaConocimiento { get; set; }
        public string areaconocimiento { get; set; }
        public int IdTitulo { get; set; }

        [Display(Name = "Institución:")]
        public string Instituacion { get; set; }
        [Display(Name = "Cargo:")]
        public string Cargo { get; set; }
        [Display(Name = "Fecha inicio:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fechainicio { get; set; }
        [Display(Name = "Fecha hasta:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fechahasta { get; set; }

        //listas
        public List<PersonaEstudio> ListasPersonaEstudio { get; set; }
        public List<ViewModelCandidatoExperiencia> ListasCanditadoExperiencia { get; set; }



    }
}
