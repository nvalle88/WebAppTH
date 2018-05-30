namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
     

    public partial class NivelDesarrollo
    {
        [Key]
        public int IdNivelDesarrollo { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<EvaluacionCompetenciasTecnicasPuesto> EvaluacionCompetenciasTecnicasPuesto { get; set; }
    }
}
