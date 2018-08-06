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

        [Required(ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Tipo de movimiento")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Sucursal")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0} ")]
        public int IdSucursal { get; set; }
        
        [Display(Name = "Puesto responsable de aprobación")]
        public int? IdManualPuesto { get; set; }

        [Display(Name = "Aprueba jefe")]
        public bool ApruebaJefe { get; set; }
        

        public virtual ManualPuesto ManualPuesto { get; set; }
        public virtual Sucursal Sucursal { get; set; }
        public virtual TipoAccionPersonal TipoAccionPersonal { get; set; }
        public virtual ICollection<AprobacionAccionPersonal> AprobacionAccionPersonal { get; set; }
    }
}
