using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class TipoDeGastoPersonal
    {
        [Key]
        public int IdTipoGastoPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        public string NombreConstante { get; set; }

        public virtual ICollection<GastoPersonal> GastoPersonal { get; set; }

    }
}
