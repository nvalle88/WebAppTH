using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
    public class DiasLaboradosNomina
    {
        [Key]
        public int IdDiasLaboradosNomina { get; set; }
        public string IdentificacionEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Días laborados")]
        [Range(0,31)]
        public int CantidadDias { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public bool Valido { get; set; }
        [Display(Name = "Descripción del error")]
        public string MensajeError { get; set; }

        public int IdEmpleado { get; set; }
        public Empleado Empleado { get; set; }


        public int IdCalculoNomina { get; set; }
        public virtual CalculoNomina CalculoNomina { get; set; }
    }
}
