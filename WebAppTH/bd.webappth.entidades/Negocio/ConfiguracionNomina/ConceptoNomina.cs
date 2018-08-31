using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class ConceptoNomina
    {
        public ConceptoNomina()
        {
            ConceptoProcesoNomina = new HashSet<ConceptoProcesoNomina>();
            FormulaNomina = new HashSet<FormulaNomina>();
        }

        public int IdConcepto { get; set; }
        [Display(Name ="Código")]
        [Required(ErrorMessage ="Debe igresar {0}")]
        [StringLength(10)]
        public string Codigo { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Debe igresar {0}")]
        [StringLength(100)]
        public string Descripcion { get; set; }

        [Display(Name = "Tipo de concepto")]
        [Range(minimum:1,maximum:double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        public int IdTipoConcepto { get; set; }

        [Display(Name = "Estado")]
        public string Estatus { get; set; }

        [Display(Name = "Procesos")]
        [Required(ErrorMessage = "Debe seleccionar {0}")]
        public int[] ProcesosSeleccionados { get; set; }


        public List<ProcesoNomina> ListaTodosProcesos { get; set; }

        public virtual ICollection<ConceptoProcesoNomina> ConceptoProcesoNomina { get; set; }
        public virtual ICollection<FormulaNomina> FormulaNomina { get; set; }
        public virtual TipoConceptoNomina TipoConceptoNomina { get; set; }
    }
}
