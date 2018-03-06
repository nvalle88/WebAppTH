using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ExamenComplementario
    {
        public int IdExamenComplementario { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public string Resultado { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Código de examen complementario")]
        public int IdTipoExamenComplementario { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Código de ficha médica")]
        public int IdFichaMedica { get; set; }
        
        public virtual FichaMedica FichaMedica { get; set; }
        public virtual TipoExamenComplementario TipoExamenComplementario { get; set; }
    }
}
