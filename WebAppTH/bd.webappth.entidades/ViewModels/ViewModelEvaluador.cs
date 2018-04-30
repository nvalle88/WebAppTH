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
        public List<ActividadesEsenciales> ListaActividad { get; set; }
        public List<AreaConocimientoViewModel> ListaConocimientos { get; set; }
        public List<ComportamientoObservableViewModel> ListaCompetenciasTecnicas { get; set; }
        public List<ComportamientoObservableViewModel> ListaCompetenciasUniversales { get; set; }


        //totales

        public int totalactividades { get; set; }

    }
}
