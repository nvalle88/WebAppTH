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

        [Display(Name = "Fecha solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Display(Name = "Hora desde")]
        public TimeSpan HoraDesde { get; set; }

        [Display(Name = "Hora hasta")]
        public TimeSpan HoraHasta { get; set; }

        [Display(Name = "Fecha desde")]
        public DateTime FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        public DateTime FechaHasta { get; set; }

        
        [Display(Name = "Motivo")]
        public string Motivo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }


        public int? Estado { get; set; }

        [Display(Name = "Permiso")]
        [Range(1,double.MaxValue,ErrorMessage = "Seleccione un {0}")]
        public int IdTipoPermiso { get; set; }

        [Display(Name = "Fecha de aprobación")]
        public DateTime? FechaAprobado { get; set; }

        public bool CargoVacaciones { get; set; }

        public virtual Empleado Empleado { get; set; }

        public virtual TipoPermiso TipoPermiso { get; set; }



    }
}
