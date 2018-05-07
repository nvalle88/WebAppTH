using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelEvaluacionDesempeno
    {
        public int IdEmpleado { get; set; }
        public int IdPersona { get; set; }
        public string NombreApellido { get; set; }
        public string Identificacion { get; set; }
        public string ManualPuesto { get; set; }
        public string Dependencia { get; set; }
        public int IdDependencia { get; set; }
        public string DatosJefe { get; set; }
        public string NombreUsuario { get; set; }
        public int IdEval001 { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDesde { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaHasta { get; set; }
    }
}
