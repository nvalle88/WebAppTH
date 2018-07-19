namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TrayectoriaLaboral
    {
        [Key]
        public int IdTrayectoriaLaboral { get; set; }

        public int IdPersona { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFin { get; set; }

        [StringLength(100)]
        public string Empresa { get; set; }

        [StringLength(100)]
        public string TipoInstitucion { get; set; }
        [StringLength(100)]
        public string FormaIngreso { get; set; }
        [StringLength(1000)]
        public string MotivoSalida { get; set; }
        [StringLength(100)]
        public string AreaAsignada { get; set; }

        [StringLength(250)]
        public string PuestoTrabajo { get; set; }

        public string DescripcionFunciones { get; set; }

        public virtual Persona Persona { get; set; }

        [NotMapped]
        [Display(Name = "Número de días")]
        public int NumeroDias { get; set; }

        [NotMapped]
        [Display(Name = "Años, meses y días")]
        public string TiempoTexto { get; set; }
    }
}
