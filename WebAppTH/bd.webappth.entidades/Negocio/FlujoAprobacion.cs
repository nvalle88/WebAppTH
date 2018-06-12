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
        public int IdTipoAccionPersonal { get; set; }
        public int IdSucursal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Puesto responsable de aprobación")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public int IdManualPuesto { get; set; }

        public virtual ManualPuesto ManualPuesto { get; set; }
        public virtual Sucursal Sucursal { get; set; }
        public virtual TipoAccionPersonal TipoAccionPersonal { get; set; }
        public virtual ICollection<AprobacionAccionPersonal> AprobacionAccionPersonal { get; set; }
    }
}
