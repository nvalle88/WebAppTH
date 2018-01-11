namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
     

    public partial class SolicitudVacaciones
    {
        [Key]
        public int IdSolicitudVacaciones { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha solicitud:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaSolicitud { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha desde:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha hasta:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaHasta { get; set; }

       
        [Display(Name = "Fecha respuesta:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FechaRespuesta { get; set; }

        [Display(Name = "Observaciones:")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Observaciones { get; set; }

        [Display(Name = "Estado:")]
        public int Estado { get; set; }

        [Display(Name = "¿Contemplado en Plan de Vacaciones?")]
        public bool PlanAnual { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Empleado:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleado { get; set; }
        public virtual Empleado Empleado { get; set; }

    }
}
