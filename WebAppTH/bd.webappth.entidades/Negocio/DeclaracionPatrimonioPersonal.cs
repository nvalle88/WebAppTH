namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DeclaracionPatrimonioPersonal
    {
        [Key]
        public int IdDeclaracionPatrimonioPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de declaración:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeclaracion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Total de efectivo:")]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal? TotalEfectivo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Total de bienes inmuebles:")]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal? TotalBienInmueble { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Total de bienes muebles:")]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal? TotalBienMueble { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Total de pasivos:")]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPasivo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Total de patrimonio")]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPatrimonio { get; set; }


        //Propiedades Virtuales Referencias a otras clases
        [Display(Name = "Sub clase de activo fijo:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEmpleado { get; set; }

        public virtual ICollection<OtroIngreso> OtroIngreso { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
