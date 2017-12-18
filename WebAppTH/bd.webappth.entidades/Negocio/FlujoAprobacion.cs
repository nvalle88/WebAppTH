using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace bd.webappth.entidades.Negocio
{
    public class FlujoAprobacion
    {
        [Key]
        public int IdFlujoAprobacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo Accion Personal:")]
        public int IdTipoAccionPersonal { get; set; }
        public virtual TipoAccionPersonal TipoAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Empleado:")]
        public int IdEmpleado { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
