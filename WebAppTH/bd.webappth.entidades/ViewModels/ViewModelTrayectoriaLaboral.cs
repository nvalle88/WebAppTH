using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelTrayectoriaLaboral
    {
        
        public int IdTrayectoriaLaboral { get; set; }

        public int IdPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha Inicio:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha Fin:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Empresa:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Puesto de Trabajo:")]
        [StringLength(205, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string PuestoTrabajo { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de Institución:")]
        //[StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TipoInstitucion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Forma de ingreso:")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string FormaIngreso { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Motivo de Salida")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string MotivoSalida { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Área Asignada")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string AreaAsignada { get; set; }


        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción de Funciones:")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string DescripcionFunciones { get; set; }

    }
}
