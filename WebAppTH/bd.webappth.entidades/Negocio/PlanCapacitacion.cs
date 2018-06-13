namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PlanCapacitacion
    {
        public int IdPlanCapacitacion { get; set; }
        public int? IdGestionPlanCapacitacion { get; set; }
        [Display(Name = "Numero partida presupuestaria ")]
        public string NumeroPartidaPresupuestaria { get; set; }
        [Display(Name = "Institución")]
        public string Institucion { get; set; }
        [Display(Name = "País")]
        public string Pais { get; set; }
        [Display(Name = "Provincia")]
        public string Provincia { get; set; }
        [Display(Name = "Ciudad")]
        public string NombreCiudad { get; set; }
        [Required]
        [Display(Name = "Nivel desconcentracion ")]
        public string NivelDesconcentracion { get; set; }
        [Display(Name = "Unidad administrativa ")]
        public string UnidadAdministrativa { get; set; }
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }
        [Display(Name = "Apellido y nombres")]
        public string ApellidoNombre { get; set; }
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
        [Display(Name = "Grupo ocupacional")]
        public string GrupoOcupacional { get; set; }
        [Display(Name = "Denominación puesto")]
        public string DenominacionPuesto { get; set; }
        [Display(Name = "Regimen laboral")]
        public string RegimenLaboral { get; set; }
        [Display(Name = "Modalidad laboral")]
        public string ModalidadLaboral { get; set; }
        [Required]
        [Display(Name = "Tema capacitación")]
        public string TemaCapacitacion { get; set; }
        [Required]
        [Display(Name = "Clasificación tema")]
        public string ClasificacionTema { get; set; }
        [Required]
        [Display(Name = "Producto final")]
        public string ProductoFinal { get; set; }
        [Required]
        [Display(Name = "Modalidad")]
        public string Modalidad { get; set; }
        [Required]
        [Display(Name = "Duración")]
        public int? Duracion { get; set; }
        [Required]
        [Display(Name = "Presupuesto individual")]
        public decimal? PresupuestoIndividual { get; set; }
        [Display(Name = "Fecha capacitacion planificada")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCapacitacionPlanificada { get; set; }
        [Required]
        [Display(Name = "Tipo capacitación")]
        public string TipoCapacitacion { get; set; }
        [Required]
        [Display(Name = "Estado evento")]
        public string EstadoEvento { get; set; }
        [Required]
        [Display(Name = "Ambito capacitación")]
        public string AmbitoCapacitacion { get; set; }
        [Required]
        [Display(Name = "Nombre evento")]
        public string NombreEvento { get; set; }
        [Required]
        [Display(Name = "Tipo evento")]
        public string TipoEvento { get; set; }
        [Required]
        public int? IdProveedorCapacitaciones { get; set; }
        [Required]
        [Display(Name = "Duración evento")]
        public int? DuracionEvento { get; set; }
        [Display(Name = "Año")]
        public int? Anio { get; set; }
        [Display(Name = "Fecha inicio")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicio { get; set; }
        [Display(Name = "Fecha fin")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFin { get; set; }
        [Required]
        [Display(Name = "Valor real")]
        public decimal? ValorReal { get; set; }
        public int? IdCiudad { get; set; }
        [Display(Name = "Tipo evaluación")]
        public string TipoEvaluacion { get; set; }
        [Required]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }
        [Required]
        [Display(Name = "Observación")]
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
        [Display(Name = "Mensaje error")]
        public string MensajeError { get; set; }
    }
}
