namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
     

    public partial class TrayectoriaLaboral
    {
        [Key]
        public int IdTrayectoriaLaboral { get; set; }

        public int IdPersona { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaInicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaFin { get; set; }

        [StringLength(100)]
        public string Empresa { get; set; }

        [StringLength(250)]
        public string PuestoTrabajo { get; set; }

        public string DescripcionFunciones { get; set; }

        public virtual Persona Persona { get; set; }
    }
}
