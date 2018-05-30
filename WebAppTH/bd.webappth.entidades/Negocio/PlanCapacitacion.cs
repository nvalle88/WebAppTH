namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PlanCapacitacion
    {
        public int IdPlanCapacitacion { get; set; }
        public int? IdGestionPlanCapacitacion { get; set; }
        public string NumeroPartidaPresupuestaria { get; set; }
        public string Institucion { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string NombreCiudad { get; set; }
        [Required]
        public string NivelDesconcentracion { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string Cedula { get; set; }
        public string ApellidoNombre { get; set; }
        public string Sexo { get; set; }
        public string GrupoOcupacional { get; set; }
        public string DenominacionPuesto { get; set; }
        public string RegimenLaboral { get; set; }
        public string ModalidadLaboral { get; set; }
        [Required]
        public string TemaCapacitacion { get; set; }
        [Required]
        public string ClasificacionTema { get; set; }
        [Required]
        public string ProductoFinal { get; set; }
        [Required]
        public string Modalidad { get; set; }
        [Required]
        public int? Duracion { get; set; }
        [Required]
        public decimal? PresupuestoIndividual { get; set; }
        [Display(Name = "Fecha capacitacion planificada")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCapacitacionPlanificada { get; set; }
        [Required]
        public string TipoCapacitacion { get; set; }
        [Required]
        public string EstadoEvento { get; set; }
        [Required]
        public string AmbitoCapacitacion { get; set; }
        [Required]
        public string NombreEvento { get; set; }
        [Required]
        public string TipoEvento { get; set; }
        [Required]
        public int? IdProveedorCapacitaciones { get; set; }
        [Required]
        public int? DuracionEvento { get; set; }
        public int? Anio { get; set; }
        [Display(Name = "Fecha inicio")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicio { get; set; }
        [Display(Name = "Fecha fin")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFin { get; set; }
        [Required]
        public decimal? ValorReal { get; set; }
        public int? IdCiudad { get; set; }
        public string TipoEvaluacion { get; set; }
        [Required]
        public string Ubicacion { get; set; }
        [Required]
        public string Observacion { get; set; }
        [NotMapped]
        public string Correo { get; set; }
        public int? Estado { get; set; }

        public virtual Ciudad Ciudad { get; set; }
        public virtual GestionPlanCapacitacion GestionPlanCapacitacion { get; set; }
        public virtual CapacitacionProveedor ProveedorCapacitaciones { get; set; }

        [NotMapped]
        public int IdEmpleado { get; set; }
        [NotMapped]
        public int IdPresupuesto { get; set; }
        [NotMapped]
        public bool Valido { get; set; }
        [NotMapped]
        public string MensajeError { get; set; }
    }
}
