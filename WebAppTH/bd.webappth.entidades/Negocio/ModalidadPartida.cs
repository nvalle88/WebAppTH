namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ModalidadPartida
    {
        [Key]
        public int IdModalidadPartida { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Modalidad de la partida")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        public virtual ICollection<IndiceOcupacionalModalidadPartida> IndiceOcupacionalModalidadPartida { get; set; }
        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimiento { get; set; }
    }
}
