using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class CeseFuncion
    {
        public int IdCeseFuncion { get; set; }
        public int IdTipoCesacionFuncion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de noticación:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaNotificacion { get; set; }

        public string Observacion { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
        public virtual TipoCesacionFuncion IdTipoCesacionFuncionNavigation { get; set; }
    }
}
