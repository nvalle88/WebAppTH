namespace bd.webappth.entidades.Negocio
{
    using bd.webappth.entidades.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AccionPersonal
    {
        [Key]
        public int IdAccionPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha:")]
        [DataType(DataType.DateTime,ErrorMessage ="Debe introducir la fecha")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número:")]
        [StringLength(20,MinimumLength =2,ErrorMessage ="El {0} no puede tener más de {1} y menos de {2}")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [DataType(DataType.Text)]
        [Display(Name = "Solicitud:")]
        public string Solicitud { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Explicación:")]
        [DataType(DataType.Text)]
        public string Explicacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Desde:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRige { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Hasta:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaRigeHasta { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Cantidad de días")]
        [Range(1, 30, ErrorMessage = "El número de días debe estar entre {1} y {2} ")]
        public int NoDias { get; set; }

        public int Estado { get; set; }

        public bool Bloquear { get; set; }
        public bool Ejecutado { get; set; }

        //Referencias a tablas

        [Display(Name = "Empleado")]
        [Range(1,double.MaxValue,ErrorMessage ="Debe seleccionar el {0} ")]
        public int IdEmpleado { get; set; }

        [Display(Name = "Tipo de Acción de personal")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoAccionPersonal { get; set; }

        [NotMapped]
        public string NombreUsuario { get; set; }

        //Propiedades Virtuales
        public virtual Empleado Empleado { get; set; }

        public virtual TipoAccionPersonal TipoAccionPersonal { get; set; }

        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimiento { get; set; }
        public virtual ICollection<AprobacionAccionPersonal> AprobacionAccionPersonal { get; set; }
    }
}
