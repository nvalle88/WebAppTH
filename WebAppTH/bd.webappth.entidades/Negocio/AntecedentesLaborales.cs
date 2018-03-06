using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class AntecedentesLaborales
    {
        public int IdAntecedentesLaborales { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Cargo que desempeñaba")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Tiempo de trabajo")]
        public string TiempoTrabajo { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Detalle de los riesgos a los que estuvo expuesto")]
        public string DetalleRiesgosExpuesto { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Epp utilizados")]
        public string EppUtilizados { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Código de ficha médica")]
        public int IdFichaMedica { get; set; }
        
        public virtual FichaMedica FichaMedica { get; set; }
    }
}
