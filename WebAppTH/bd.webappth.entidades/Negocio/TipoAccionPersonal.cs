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
        [Display(Name = "Tipo de acci�n de personal")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "N�mero d�as m�nimo")]
        public int NDiasMinimo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "N�mero d�as M�ximo")]
        public int NDiasMaximo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "N�mero horas m�nimo")]
        public int NHorasMinimo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "N�mero horas m�ximo")]
        public int NHorasMaximo { get; set; }
        
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "D�as h�biles")]
        public bool DiasHabiles { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Imputable vacaciones")]
        public bool ImputableVacaciones { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Proceso n�mina")]
        public bool ProcesoNomina { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Es responsable TH")]
        public bool EsResponsableTH { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Matriz")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Debe seleccionar {0}")]
        public string Matriz { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripci�n")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Genera acci�n personal")]
        public bool GeneraAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Modifica distributivo")]
        public bool ModificaDistributivo { get; set; }
        

        public int MesesMaximo { get; set; }
        public int YearsMaximo { get; set; }
        public bool DesactivarCargo { get; set; }
        public bool Definitivo { get; set; }
        public bool DesactivarEmpleado { get; set; }
        public bool ModalidadContratacion { get; set; }
        

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<AccionPersonal> AccionPersonal { get; set; }
        
        public virtual ICollection<FlujoAprobacion> FlujoAprobacion { get; set; }

        public virtual ICollection<PieFirma> PieFirma { get; set; }

    }
}
