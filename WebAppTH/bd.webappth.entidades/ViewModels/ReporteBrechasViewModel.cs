using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ReporteBrechasViewModel
    {

        public int IdRol { get; set; }
        

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Rol:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string NombreRol { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Brecha:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Brecha { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Observaciones:")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Observaciones { get; set; }

    }
}
