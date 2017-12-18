using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace bd.webappth.entidades.Negocio
{
    public class PieFirma
    {
        [Key]
        public int IdPieFirma { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo Accion Personal:")]
        public int IdTipoAccionPersonal { get; set; }
        public virtual TipoAccionPersonal TipoAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Indice Ocupacional:")]
        public int IdIndiceOcupacional { get; set; }
        public virtual IndiceOcupacional IndiceOcupacional { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nivel:")]
        public int Nivel { get; set; }
    }
}
