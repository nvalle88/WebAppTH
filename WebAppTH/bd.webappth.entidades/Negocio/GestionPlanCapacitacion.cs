namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class GestionPlanCapacitacion
    {
        public GestionPlanCapacitacion()
        {
            PlanCapacitacion = new HashSet<PlanCapacitacion>();
        }

        public int IdGestionPlanCapacitacion { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Año")]
        public int? Anio { get; set; }
        [Display(Name = "Estado")]
        public int? Estado { get; set; }
        public virtual ICollection<PlanCapacitacion> PlanCapacitacion { get; set; }
        [NotMapped]
        public string NombreUsuario { get; set; }
    }
}
