using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class HorasExtrasNomina
    {
        [Key]
        public int IdHorasExtrasNomina { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Cantidad de horas")]
        public int CantidadHoras { get; set; }

        public string IdentificacionEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "¿ Horas extraordinarias ?")]
        public bool EsExtraordinaria { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Empleado")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleado { get; set; }


        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public bool Valido { get; set; }
        [Display(Name = "Descripción del error")]
        public string MensajeError { get; set; }
        /// <summary>
        /// propiedades virtuales relaciones con otras tablas
        /// </summary>
        public int IdCalculoNomina { get; set; }
        public virtual CalculoNomina CalculoNomina { get; set; }
    }
}
