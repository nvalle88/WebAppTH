namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
     

    public partial class SolicitudHorasExtras
    {
        [Key]
        public long IdSolicitudHorasExtras { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de registro:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Fecha de la solicitud:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FechaSolicitud { get; set; }

       
        [Display(Name = "Fecha de aprobado:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FechaAprobado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Horas:")]
        [Range(1, 8, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? CantidadHoras { get; set; }

        [Display(Name = "Observaciones:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Estado")]
        public int? Estado { get; set; }

        [Display(Name = "¿Son horas extraordinarias?")]
        public bool EsExtraordinaria{ get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public int IdEmpleado { get; set; }
        public virtual Empleado Empleado { get; set; }

    }
}
