using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{ 
    public class ListaEmpleadoViewModel
    {
        public int IdEmpleado { get; set; }
        public int IdPersona { get; set; }

        [Display(Name = "Nombres y apellidos")]
        public string NombreApellido { get; set; }

        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }
        public string TelefonoPrivado { get; set; }
        public string CorreoPrivado { get; set; }

        [Display(Name = "Unidad administrativa")]
        public string Dependencia { get; set; }
        public string RolPuesto { get; set; }
        public bool TipoCuenta { get; set; }
        public string NoCuenta { get; set; }
        public string InstitucionBancaria { get; set; }
        public string FondoFinanciamiento { get; set; }
        public int IdConfiguracionViatico { get; set; }
        public DateTime FechaIngreso { get; set; }

        public int IdRelacionLaboral { get; set; }

        [Display(Name = "Relación laboral")]
        public string NombreRelacionLaboral { get; set; }

        public int IdManualPuesto { get; set; }

        [Display(Name = "Cargo")]
        public string ManualPuesto { get; set; }

        [Display(Name = "Partida individual / Código")]
        public string PartidaIndividual { get; set; }
    }
}
