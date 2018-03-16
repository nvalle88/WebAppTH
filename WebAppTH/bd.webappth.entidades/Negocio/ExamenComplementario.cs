using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ExamenComplementario
    {
        public int IdExamenComplementario { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public string Resultado { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Código de examen complementario")]
        public int IdTipoExamenComplementario { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Código de ficha médica")]
        public int IdFichaMedica { get; set; }

        [Display(Name = "Seleccione un archivo")]
        public string Url { get; set; }

        public virtual FichaMedica FichaMedica { get; set; }
        public virtual TipoExamenComplementario TipoExamenComplementario { get; set; }
    }
}
