namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
    public partial class SolicitudPermiso
    {
        [Key]
        public int IdSolicitudPermiso { get; set; }

        public int IdEmpleado { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public TimeSpan HoraDesde { get; set; }

        public TimeSpan HoraHasta { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Motivo:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Motivo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Observacion:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Observacion { get; set; }

        public int? Estado { get; set; }

        public int IdTipoPermiso { get; set; }

        public DateTime? FechaAprobado { get; set; }
        
        public virtual Empleado Empleado { get; set; }

        public virtual TipoPermiso TipoPermiso { get; set; }



    }
}
