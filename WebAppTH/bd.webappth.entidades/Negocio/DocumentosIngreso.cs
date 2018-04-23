using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class DocumentosIngreso
    {
      

        public int IdDocumentosIngreso { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual ICollection<DocumentosIngresoEmpleado> DocumentosIngresoEmpleado { get; set; }
    }
}
