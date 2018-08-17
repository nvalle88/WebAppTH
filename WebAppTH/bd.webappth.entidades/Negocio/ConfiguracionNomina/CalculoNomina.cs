using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class CalculoNomina
    {
        [Key]
        public int IdCalculoNomina { get; set; }

        [Display(Name = "Activo")]
        public bool EmpleadoActivo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de la nómina:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNomina { get; set; }

        [Display(Name = "Fecha de inicio:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicioDecimoTercero { get; set; }


        [Display(Name = "Fecha final:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFinDecimoTercero { get; set; }



        [Display(Name = "Fecha de inicio:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicioDecimoCuarto { get; set; }


        [Display(Name = "Fecha final:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFinDecimoCuarto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Url")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Url { get; set; }

        public bool DecimoTercerSueldo { get; set; }

        public string DecimoCuartoSueldo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Estado")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? Estado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Proceso")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProceso { get; set; }
        public virtual ProcesoNomina ProcesoNomina { get; set; }

        public virtual ICollection<ReportadoNomina> ReportadoNomina { get; set; }
        public virtual ICollection<CabeceraNomina> CabeceraNomina { get; set; }
        public virtual ICollection<DiasLaboradosNomina> DiasLaboradosNomina { get; set; }
    }
}
