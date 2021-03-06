namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class FormulasRMU
    {
        [Key]
        public int IdFormulaRMU { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Formula")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Formula { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<IngresoEgresoRMU> IngresoEgresoRMU { get; set; }
    }
}
