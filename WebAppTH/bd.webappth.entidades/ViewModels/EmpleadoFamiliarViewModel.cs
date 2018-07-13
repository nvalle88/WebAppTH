using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class EmpleadoFamiliarViewModel : IValidatableObject
    {
        public int IdEmpleado { get; set; }
        public int IdEmpleadoFamiliar { get; set; }
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de identificación")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdTipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Identificación ")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Parentesco ")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdParentesco { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nombres ")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Apellidos ")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Sexo ")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdSexo { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Género ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdGenero { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Estado Cívil ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdEstadoCivil { get; set; }

        [Display(Name = "Correo privado ")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string CorreoPrivado { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha de Nacimiento ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Lugar de trabajo ")]
        //[StringLength(500, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string LugarTrabajo { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Celular ")]
        //[StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoPrivado { get; set; }

        [Display(Name = "Teléfono de casa ")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string TelefonoCasa { get; set; }

        //[Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Ocupación ")]
        //[StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Ocupacion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (IdTipoIdentificacion == 1)
            {
                var cad = Identificacion.ToString();
                var total = 0;
                var longitud = cad.Length;
                var longcheck = longitud - 1;

                if (cad != "" && longitud == 10)
                {
                    for (int i = 0; i < longcheck; i++)
                    {
                        if (i % 2 == 0)
                        {
                            var aux = int.Parse(cad.Substring(i, 1)) * 2;
                            if (aux > 9)
                            {
                                aux -= 9;
                            }
                            total += aux;

                        }
                        else
                        {
                            total += int.Parse(cad.Substring(i, 1)); // parseInt o concatenará en lugar de sumar
                        }
                    }

                    total = total % 10;
                    total = 10 - total;

                    if (int.Parse(cad.Substring(longitud - 1, 1)) != total)
                    {
                        yield return
                      new ValidationResult(errorMessage: "La cédula no es válida",
                                           memberNames: new[] { "Identificacion" });
                    }
                }
            }

        }
    }
}