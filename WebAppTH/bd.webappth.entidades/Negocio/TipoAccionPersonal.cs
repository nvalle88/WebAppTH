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


        public bool Definitivo { get; set; }
        public bool Dias { get; set; }
        public int YearsMaximo { get; set; }
        public int MesesMaximo { get; set; }


        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número días mínimo")]
        public int NdiasMinimo { get; set; }


        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número días Máximo")]
        public int NdiasMaximo { get; set; }



        public bool Horas { get; set; }


        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número horas mínimo")]
        public int NhorasMinimo { get; set; }


        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número horas máximo")]
        public int NhorasMaximo { get; set; }


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
        public bool EsResponsableTh { get; set; }


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



        public bool GenerarMovimientoPersonal { get; set; }



        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Modifica distributivo")]
        public bool ModificarDistributivo { get; set; }


        public bool DesactivarCargo { get; set; }
        public bool DesactivarEmpleado { get; set; }
        public bool FinalizaTipoAccionPersonal { get; set; }
        public int? IdTipoAccionPersonalFin { get; set; }

        public bool? BuscarPuestosVacantes { get; set; }
        public bool? BuscarPuestosOcupados { get; set; }
        public bool? BuscarNivelOperativo { get; set; }
        public bool? BuscarJerarquicoSuperior { get; set; }



        public virtual ICollection<AccionPersonal> AccionPersonal { get; set; }
        public virtual ICollection<FlujoAprobacion> FlujoAprobacion { get; set; }
        public virtual ICollection<PieFirma> PieFirma { get; set; }



        

        
        

        

        

        
        
        
        
        
        
    }
}
