using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class OtroIngreso
    {
        public int IdOtroIngreso { get; set; }
        public int IdDeclaracionPatrimonioPersonal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ingresos de Cónyuge:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? IngresoConyuge { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ingresos de arriendos:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? IngresoArriendos { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ingresos de negocio particular:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? IngresoNegocioParticular { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ingresos de rentas financieras:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? IngresoRentasFinancieras { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Otros ingresos:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? OtrosIngresos { get; set; }

        [Display(Name = "Descripción de otros ingresos:")]
        public string DescripcionOtros { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Total de otros ingresos :")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? Total { get; set; }

        public virtual DeclaracionPatrimonioPersonal IdDeclaracionPatrimonioPersonalNavigation { get; set; }
    }
}
