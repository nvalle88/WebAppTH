namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Parroquia
    {
        [Key]
        public int IdParroquia { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Parroquia:")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Nombre { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        [Display(Name = "Ciudad:")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdCiudad { get; set; }

        [Display(Name = "Pais:")]
        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdPais { get; set; }

        [Display(Name = "Provincia:")]
        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProvincia { get; set; }


        public virtual Ciudad Ciudad { get; set; }

        public ICollection<Persona> Persona { get; set; }
    }
}
