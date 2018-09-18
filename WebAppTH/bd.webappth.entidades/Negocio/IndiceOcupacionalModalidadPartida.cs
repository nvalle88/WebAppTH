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
        public int IdRelacionLaboral { get; set; }
        public string CodigoContrato { get; set; }
        public string NumeroPartidaIndividual { get; set; }
        public int IdDependencia { get; set; }
        public int IdIndiceOcupacional { get; set; }
        public int? IdModalidadPartida { get; set; }
        public int? IdGrupoOcupacionalSobrevalorado { get; set; }
        public int? IdEscalaGradosSobrevalorado { get; set; }
        public decimal? Rmusobrevalorado { get; set; }
        public bool Activo { get; set; }
        public bool EsJefe { get; set; }
        public bool Ocupado { get; set; }


        public virtual ICollection<DistributivoHistorico> DistributivoHistorico { get; set; }
        public virtual ICollection<DistributivoSituacionActual> DistributivoSituacionActual { get; set; }


        public virtual Dependencia Dependencia { get; set; }
        public virtual EscalaGrados EscalaGradosSobrevalorado { get; set; }
        public virtual GrupoOcupacional GrupoOcupacionalSobrevalorado { get; set; }
        public virtual IndiceOcupacional IndiceOcupacional { get; set; }
        public virtual ModalidadPartida ModalidadPartida { get; set; }
        public virtual RelacionLaboral RelacionLaboral { get; set; }
        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimientoIompDesde { get; set; }
        public virtual ICollection<EmpleadoMovimiento> EmpleadoMovimientoIompHasta { get; set; }


    }
}
