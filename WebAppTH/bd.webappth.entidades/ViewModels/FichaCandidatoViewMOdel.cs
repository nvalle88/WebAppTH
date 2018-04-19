using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class FichaCandidatoViewModel
    {

        public int IdCandidato { get; set; }

        public int IdPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Identificación:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombres:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Apellidos:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono privado:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono de la Casa:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoCasa { get; set; }

        [Display(Name = "Correo privado ")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string CorreoPrivado { get; set; }
    }
}
