namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IndiceOcupacionalModalidadPartida
    {
        [Key]
        public int IdIndiceOcupacionalModalidadPartida { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Salario real")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? SalarioReal { get; set; }
        

        [Display(Name = "Índice ocupacional")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int IdIndiceOcupacional { get; set; }
        public virtual IndiceOcupacional IndiceOcupacional { get; set; }

        [Display(Name = "Empleado")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdEmpleado { get; set; }
        public virtual Empleado Empleado { get; set; }

        [Display(Name = "Fondo de financiamiento")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdFondoFinanciamiento { get; set; }
        public virtual FondoFinanciamiento FondoFinanciamiento { get; set; }

        [Display(Name = "Tipo de nombramiento / contrato")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0} ")]
        public int? IdTipoNombramiento { get; set; }
        public virtual TipoNombramiento TipoNombramiento { get; set; }
        
        
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El campo {0}, no debe estar vacío")]
        public string CodigoContrato { get; set; }

        public int? IdModalidadPartida { get; set; }
        public virtual ModalidadPartida ModalidadPartida { get; set; }

        [Display(Name = "Partida individual")]
        public string NumeroPartidaIndividual { get; set; }


        [Display(Name = "Fecha fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFin { get; set; }

        [NotMapped]
        public int IdDependencia { get; set; }


        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimientoIdIndiceOcupacionalModalidadPartidaDesde { get; set; }
        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimientoIdIndiceOcupacionalModalidadPartidaHasta { get; set; }

    }
}
