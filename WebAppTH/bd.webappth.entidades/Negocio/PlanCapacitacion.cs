namespace bd.webappth.entidades.Negocio
{
    using System;

    public partial class PlanCapacitacion
    {
        public int IdPlanCapacitacion { get; set; }
        public string NumeroPartidaPresupuestaria { get; set; }
        public string Institucion { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public string NivelDesconcentracion { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string Cedula { get; set; }
        public string ApellidoNombre { get; set; }
        public string Sexo { get; set; }
        public string GrupoOcupacional { get; set; }
        public string DenominacionPuesto { get; set; }
        public string RegimenLaboral { get; set; }
        public string ModalidadLaboral { get; set; }
        public string TemaCapacitacion { get; set; }
        public string ClasificacionTema { get; set; }
        public string ProductoFinal { get; set; }
        public string Modalidad { get; set; }
        public decimal? PresupuestoIndividual { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
