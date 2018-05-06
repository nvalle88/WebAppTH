using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelEvaluador
    {
        public int IdEmpleado { get; set; }
        public int IdEval001 { get; set; }
        public int IdJefe { get; set; }
        public int IdIndiceOcupacional { get; set; }
        public int IdNivelConocimiento { get; set; }
        public int IdNivelDesarrollo { get; set; }
        public int IdFrecuenciaAplicacion { get; set; }
        public int OpcionMenu { get; set; }

        [Display(Name = "Apellido y Nombres:")]
        public string NombreApellido { get; set; }
        [Display(Name = "Puesto")]
        public string Puesto { get; set; }
        [Display(Name = "Titulo o Profesión:")]
        public string Titulo { get; set; }
        [Display(Name = "Apellido y Nombres Jefe Inmediato:")]
        public string DatosJefe { get; set; }

        [Display(Name = "Fecha inicio:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Desde { get; set; }

        [Display(Name = "Fecha Hasta:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Hasta { get; set; }

        public string NombreUsuario { get; set; }
        public string Cuatroporciento { get; set; }
        public string PorcentajeAumento { get; set; }
        public List<string> ListaActividades { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        public List<string> ListaIndicadores{ get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        public List<string> ListaMetaPeriodo { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        public List<string> ListaActividadescumplidos { get; set; }
        public List<string> IdAreaConocimiento { get; set; }
        public List<string> ConocimientosEsenciales { get; set; }
        public List<string> CompetenciasTecnicas { get; set; }
        public List<string> PorcentajeCumplido { get; set; }
        
        public List<string> NivelCumplimiento { get; set; }
        public List<string> IdFrecuenciaAplicaciones { get; set; }

        //Competencias Tecnicas Puesto
        public List<string> IdComportamientoObervable { get; set; }
        public List<string> IdNivelDesarrollos { get; set; }

        public List<ActividadesEsenciales> ListaActividad { get; set; }
        public List<AreaConocimientoViewModel> ListaConocimientos { get; set; }
        public List<ComportamientoObservableViewModel> ListaCompetenciasTecnicas { get; set; }
        public List<ComportamientoObservableViewModel> ListaCompetenciasUniversales { get; set; }
        public List<ComportamientoObservableViewModel> ListaEquipoLiderazgo { get; set; }
        //Observaciones

        public string Observaciones { get; set; }

        //totalesActividades

        public int totalactividades { get; set; }
        public int TotalConocimiento { get; set; }


        // totales
        public double ActividadesTotal { get; set; }
        public double TotalConocimientos { get; set; }
        public double TotalCompetenciasTecnicas { get; set; }
        public double TotalCompetenciasUniversales { get; set; }
        public double TotalTrabajoLiderazgo { get; set; }
        public double TotalQuejas { get; set; }
        public double TotalEvaluacion { get; set; }

    }
}
