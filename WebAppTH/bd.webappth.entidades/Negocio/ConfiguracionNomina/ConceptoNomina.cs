using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ConceptoNomina
    {
        public ConceptoNomina()
        {
            ConceptoConjuntoNomina = new HashSet<ConceptoConjuntoNomina>();
            TeconceptoNomina = new HashSet<TeconceptoNomina>();
        }

        public int IdConcepto { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Proceso")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdProceso { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de concepto")]
        public string TipoConcepto { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Tipo de cálculo")]
        public string TipoCalculo { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Nivel de acumulación")]
        public string NivelAcumulacion { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Registro en")]
        public string RegistroEn { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Estatus")]
        public string Estatus { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Abreviatura")]
        public string Abreviatura { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Prioridad")]
        [Range(1,int.MaxValue,ErrorMessage ="La {0} no puede ser menor que {1} ni mayor que {2}")]
        public int Prioridad { get; set; }

        public string FormulaCalculo { get; set; }

        public virtual ICollection<ConceptoConjuntoNomina> ConceptoConjuntoNomina { get; set; }
        public virtual ICollection<TeconceptoNomina> TeconceptoNomina { get; set; }
        public virtual ProcesoNomina ProcesoNomina { get; set; }

    }
}
