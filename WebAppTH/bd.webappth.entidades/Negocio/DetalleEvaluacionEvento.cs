namespace bd.webappth.entidades.Negocio
{

    public partial class DetalleEvaluacionEvento
    {
        public int IdDetalleEvaluacionEvento { get; set; }
        public int? Calificacion { get; set; }
        public int IdPreguntasEvaluacionEvento { get; set; }
        public int? IdEvaluacionEvento { get; set; }
        public bool? Conocimiento { get; set; }

        public virtual EvaluacionEvento EvaluacionEvento { get; set; }
        public virtual PreguntaEvaluacionEvento PreguntasEvaluacionEvento { get; set; }
    }
}
