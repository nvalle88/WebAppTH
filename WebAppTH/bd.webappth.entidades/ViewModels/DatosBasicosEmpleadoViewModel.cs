﻿using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DatosBasicosEmpleadoViewModel : IValidatableObject
    {
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de identificación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Identificación")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombres")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Apellidos")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Sexo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdSexo { get; set; }

        [Display(Name = "Acumula fondo de reserva")]
        public bool FondosReservas { get; set; }

        [Display(Name = "Derecho fondo de reserva")]
        public bool DerechoFondoReserva { get; set; }

        [Display(Name = "Décimo tercero")]
        public bool AcumulaDecimoTercero { get; set; }

        [Display(Name = "Décimo cuarto")]
        public bool AcumulaDecimoCuarto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Género")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdGenero { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Estado cívil")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEstadoCivil { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de sangre")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoSangre { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nacionalidad")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdNacionalidad { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Etnia")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEtnia { get; set; }

        public int? IdNacionalidadIndigena { get; set; }

        public int? IdBrigadaSSoRol { get; set; }
        public int? IdBrigadaSSO { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Correo privado")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string CorreoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Lugar de trabajo")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string LugarTrabajo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "País")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPaisLugarNacimiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ciudad")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdCiudadLugarNacimiento { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "País")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPaisLugarSufragio { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Provincia")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProvinciaLugarSufragio { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "País")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPaisLugarPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Provincia")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProvinciaLugarPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ciudad")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdCiudadLugarPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Parroquia")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdParroquia { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Calle principal")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string CallePrincipal { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Calle secundaria")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string CalleSecundaria { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Referencia")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Número de casa")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono celular")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Teléfono de casa")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoCasa { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ocupación")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Ocupacion { get; set; }

        [Display(Name = "Tipo relación")]
        public string RelacionSuperintendencia { get; set; }

        public List<BrigadaSSORol> BrigadaSSORol { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Now < FechaNacimiento.AddYears(16).Date)
            {
                yield return
                  new ValidationResult(errorMessage: "La persona debe tener más de 16 años",
                                       memberNames: new[] { "FechaNacimiento" });
            }


            if (IdTipoIdentificacion == 1)
            {
                var cad = Identificacion.ToString();
                var longitud = cad.Length;
                var longcheck = longitud - 1;

                if (cad != "" && longitud != 10)
                {
                    yield return
                       new ValidationResult(errorMessage: "La cédula no es válida",
                                            memberNames: new[] { "Identificacion" });
                }
               
            }

        }
    }
}
