using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class AntecedentesFamiliares
    {
        public int IdAntecedentesFamiliares { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public string Parentesco { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public string Enfermedad { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Código de ficha médica")]
        public int IdFichaMedica { get; set; }

        public virtual FichaMedica FichaMedica { get; set; }
    }
}
