using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelQuejas
    {
        public int IdQuejas { get; set; }
        public int IdEmpleado { get; set; }
        public int IdEval001 { get; set; }
        public int IdJefe { get; set; }
        public int IdIndiceOcupacional { get; set; }
        public int IdNivelConocimiento { get; set; }
        public int IdNivelDesarrollo { get; set; }
        public int IdFrecuenciaAplicacion { get; set; }
        public int OpcionMenu { get; set; }
        [Display(Name = "Nombres:")]
        public string NombreCiudadano { get; set; }
        [Display(Name = "Apellido:")]
        public string ApellidoCiudadano { get; set; }
        public string Descripcion { get; set; }
        public int NumeroFormulario { get; set; }
        public bool AplicaDescuento { get; set; }

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
