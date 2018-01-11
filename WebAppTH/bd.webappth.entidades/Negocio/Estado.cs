namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Estado
    {
        public Estado()
        {
            RecepcionActivoFijoDetalle = new HashSet<RecepcionActivoFijoDetalle>();
            SolicitudAnticipo = new HashSet<SolicitudAnticipo>();
            SolicitudModificacionFichaEmpleado = new HashSet<SolicitudModificacionFichaEmpleado>();
            SolicitudViatico = new HashSet<SolicitudViatico>();
        }
        [Key]
        public int IdEstado { get; set; }
        public int? IdSolicitudCertificadoPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener m�s de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        //[Display(Name = "Solicitud certificado personal:")]
        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        //public int? IdSolicitudCertificadoPersonal { get; set; }
        //public virtual SolicitudCertificadoPersonal SolicitudCertificadoPersonal1 { get; set; }

        public virtual ICollection<SolicitudPermiso> SolicitudPermiso { get; set; }


        public virtual ICollection<RecepcionActivoFijoDetalle> RecepcionActivoFijoDetalle { get; set; }
        public virtual ICollection<SolicitudAnticipo> SolicitudAnticipo { get; set; }
        public virtual ICollection<SolicitudModificacionFichaEmpleado> SolicitudModificacionFichaEmpleado { get; set; }
        public virtual ICollection<SolicitudViatico> SolicitudViatico { get; set; }

        public virtual ICollection<SolicitudModificacionFichaEmpleado> SolicitudModificacionFichaEmpleado { get; set; }

        public virtual ICollection<SolicitudAnticipo> SolicitudAnticipo { get; set; }

        public virtual ICollection<SolicitudCertificadoPersonal> SolicitudCertificadoPersonal { get; set; }

        
    }
}
