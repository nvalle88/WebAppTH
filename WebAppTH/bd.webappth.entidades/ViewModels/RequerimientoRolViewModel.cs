using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class RequerimientoRolViewModel : IValidatableObject
    {
        public int IdRolPuesto { get; set; }

        [Display(Name = "Puesto")]
        public string NombreRolPuesto { get; set; }

        //[Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        //[Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Cantidad > 0 && String.IsNullOrEmpty(Descripcion))
            {
                yield return
                  new ValidationResult(errorMessage: "Se debe agregar una descripción",
                                       memberNames: new[] { "Descripcion" });
            }

            if (!String.IsNullOrEmpty(Descripcion) && Cantidad < 1) {

                yield return
                  new ValidationResult(errorMessage: "No se ha ingresado cantidad",
                                       memberNames: new[] { "Cantidad" });
            }

        }
    }
}
