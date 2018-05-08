namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
    public partial class TipoAccionPersonal
    {
        [Key]
        public int IdTipoAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de acción de personal")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número días mínimo")]
        public int NDiasMinimo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número días Máximo")]
        public int NDiasMaximo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número horas mínimo")]
        public int NHorasMinimo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número horas máximo")]
        public int NHorasMaximo { get; set; }
        
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Días hábiles")]
        public bool DiasHabiles { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Imputable vacaciones")]
        public bool ImputableVacaciones { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Proceso nómina")]
        public bool ProcesoNomina { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Es responsable TH")]
        public bool EsResponsableTH { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Matriz")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Debe seleccionar {0}")]
        public string Matriz { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Genera acción personal")]
        public bool GeneraAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Modifica distributivo")]
        public bool ModificaDistributivo { get; set; }
        
        
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        [Display(Name = "Cambio de estado")]
        public int IdEstadoTipoAccionPersonal { get; set; }

        public virtual EstadoTipoAccionPersonal EstadoTipoAccionPersonal { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<AccionPersonal> AccionPersonal { get; set; }
    }
}
