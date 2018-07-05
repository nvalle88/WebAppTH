namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
     

    public partial class ManualPuesto
    {
        [Key]
        public int IdManualPuesto { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [StringLength(50)]

        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir  {0}")]
        [Display(Name = "Descripci�n")]
        public string Descripcion { get; set; }

        [Display(Name = "Misi�n")]
        public string Mision { get; set; }

        [Display(Name = "Relaciones internas y externas")]
        public int? IdRelacionesInternasExternas { get; set; }

        public virtual ICollection<IndiceOcupacional> IndiceOcupacional { get; set; }
        public virtual ICollection<InformeUATH> InformeUATH { get; set; }

        public virtual ICollection<InformeUATH> InformeUATH1 { get; set; }

        public virtual RelacionesInternasExternas RelacionesInternasExternas { get; set; }
    }
}
