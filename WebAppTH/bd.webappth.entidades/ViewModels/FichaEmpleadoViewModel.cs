using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class FichaEmpleadoViewModel:DatosBasicosEmpleadoViewModel
    {
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ingreso sector público:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaIngresoSectorPublico { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Extención telefónica:")]
        public int? ExtencionTelefonica { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono")]
        
        public string Telefono { get; set; }

        [Display(Name = "Meses de imposiciones al IESS:")]
        [Range(0, 1000, ErrorMessage = "Los {0} no pueden ser mayor a {2} ")]
        public int MesesImposiciones { get; set; }

        [Display(Name = " Días de imposiciones al IESS:")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} no es un valor numérico")]
        [Range(0, 31, ErrorMessage = "Los {0} no pueden ser mayor a {2} ")]
        public int DiasImposiciones { get; set; }

        [Display(Name = "Fondo de reserva")]
        public bool FondosReservas { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Cargo en la brigada")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdBrigadaSSORol { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Brigada")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdBrigadaSSO { get; set; }

        [Display(Name = "¿Trabajó en la Super de bancos?")]
        public bool TrabajoSuperintendenciaBanco { get; set; }

        [Display(Name = "¿Declaración jurada?")]
        public bool DeclaracionJurada { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "¿Es Jefe?")]
        public bool EsJefe { get; set; }

        [Display(Name = "¿Nepotismo?")]
        public bool Nepotismo { get; set; }

        [Display(Name = "¿Otros ingresos?")]
        public bool OtrosIngresos { get; set; }

        [Display(Name = "Ingreso de otra actividad")]
        public string IngresosOtraActividad { get; set; }

        [Display(Name = "¿Detalle?")]
        public string Detalle { get; set; }

        [Display(Name = "Año de  desvinculación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public int? AnoDesvinculacion { get; set; }

        [Display(Name = "Tipo relación")]
        public string TipoRelacion { get; set; }

    }
}
