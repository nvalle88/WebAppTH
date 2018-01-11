using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ConfiguracionFeriados
    {
        public int IdConfiguracionFeriado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha desde:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha hasta:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaHasta { get; set; }

        [Display(Name = "Descripcion:")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }
    }
}
