using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace bd.webappth.entidades.Negocio
{
    public class EstadoTipoAccionPersonal
    {
        [Key]
        public int IdEstadoTipoAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre:")]
        public string Nombre { get; set; }
    }
}
