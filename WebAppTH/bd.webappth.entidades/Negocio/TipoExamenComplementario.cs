using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class TipoExamenComplementario
    {
        public TipoExamenComplementario()
        {
            ExamenComplementario = new HashSet<ExamenComplementario>();
        }

        [Display(Name = "Código de examen complementario")]
        public int IdTipoExamenComplementario { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        public virtual ICollection<ExamenComplementario> ExamenComplementario { get; set; }
    }
}
