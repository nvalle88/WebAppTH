namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IndiceOcupacional
    {
        [Key]
        public int IdIndiceOcupacional { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Denominación del puesto")]
        public string DenominacionPuesto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Unidad administrativa")]
        public string UnidadAdministrativa { get; set; }
        
        [Display(Name = "Rol del puesto")]
        [Range(1,double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdRolPuesto { get; set; }

        [Display(Name = "Escala de grados")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar una opción ")]
        public int? IdEscalaGrados { get; set; }

        public int? IdPartidaGeneral { get; set; }

        [Display(Name = "Ámbito")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdAmbito { get; set; }

        
        [Required(ErrorMessage = "Debe seleccionar el {0}")]
        public string Nivel { get; set; }

        [Display(Name = "Misión")]
        public string Mision { get; set; }

        [Display(Name = "Relaciones internas externas")]
        public string RelacionesInternasExternas { get; set; }

        public bool SinClasificar { get; set; }
        public decimal? RmusinClasificar { get; set; }
        public bool Activo { get; set; }
        

        public virtual ICollection<IndiceOcupacionalActividadesEsenciales> IndiceOcupacionalActividadesEsenciales { get; set; }
        public virtual ICollection<IndiceOcupacionalAreaConocimiento> IndiceOcupacionalAreaConocimiento { get; set; }
        public virtual ICollection<IndiceOcupacionalCapacitaciones> IndiceOcupacionalCapacitaciones { get; set; }
        public virtual ICollection<IndiceOcupacionalComportamientoObservable> IndiceOcupacionalComportamientoObservable { get; set; }
        public virtual ICollection<IndiceOcupacionalConocimientosAdicionales> IndiceOcupacionalConocimientosAdicionales { get; set; }
        public virtual ICollection<IndiceOcupacionalEstudio> IndiceOcupacionalEstudio { get; set; }
        public virtual ICollection<IndiceOcupacionalExperienciaLaboralRequerida> IndiceOcupacionalExperienciaLaboralRequerida { get; set; }
        public virtual ICollection<IndiceOcupacionalModalidadPartida> IndiceOcupacionalModalidadPartida { get; set; }
        public virtual ICollection<PartidasFase> PartidasFase { get; set; }
        public virtual ICollection<PieFirma> PieFirma { get; set; }
        public virtual Ambito Ambito { get; set; }
        public virtual EscalaGrados EscalaGrados { get; set; }
        public virtual PartidaGeneral PartidaGeneral { get; set; }
        public virtual RolPuesto RolPuesto { get; set; }



        
        
    }
}
