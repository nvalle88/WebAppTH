namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TipoNombramiento
    {
        [Key]
        public int IdTipoNombramiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "tipo de nombramiento:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

       

        [Display(Name = "Relaci�n laboral:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdRelacionLaboral { get; set; }
        public virtual RelacionLaboral RelacionLaboral { get; set; }

        public virtual ICollection<IndiceOcupacionalModalidadPartida> IndiceOcupacionalModalidadPartida { get; set; }
        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimiento { get; set; }

    }
}
