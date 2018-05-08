using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace bd.webappth.entidades.Negocio
{
    public class EstadoTipoAccionPersonal
    {
        [Key]
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Cambio de estado")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEstadoTipoAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Cambio de estado")]
        public string Nombre { get; set; }
    }
}
