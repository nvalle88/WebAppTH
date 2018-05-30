namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EvaluacionActividadesPuestoTrabajo
    {
        [Key]
        public int IdEvaluacionActividadesPuestoTrabajo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ActividadesEsenciales ActividadesEsenciales { get; set; }
        public virtual Eval001 Eval001 { get; set; }
        public virtual Indicador Indicador { get; set; }
    }
}
