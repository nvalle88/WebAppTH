using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelEvaluacionCapacitaciones
    {
        public int IdEmpleado { get; set; }
        public int IdPlanCapacitacion { get; set; }
        public string NombreUsuario { get; set; }
        [DisplayName("Nombre evento")]
        public string NombreEvento { get; set; }
        [DisplayName("Institución")]
        public string Institucion { get; set; }
        [DisplayName("Lugar y Fecha")]
        public string LugarFecha { get; set; }
        [DisplayName("Comentarios y Sugerencias")]
        public string ComentarioSugerencia { get; set; }
        //LIstas
        public List<DetalleEvaluacionEvento> ListaDetalleEvaluacionEvento { get; set; }
        public List<PlanCapacitacion> ListaPlanCapacitacion { get; set; }
        public List<PreguntaEvaluacionEvento> ListaPreguntaEvaluacionEvento { get; set; }
        public List<PreguntaEvaluacionEvento> ListaPreguntaEvaluacionFacilitador { get; set; }
        public List<PreguntaEvaluacionEvento> ListaPreguntaOrganizador { get; set; }
        public List<PreguntaEvaluacionEvento> ListaPreguntaEvaluacionConocimiento{ get; set; }
        //public List<string> PreguntaFacilitador { get; set; }
        //public List<string> PreguntaOrganizador { get; set; }
        //public List<string> PreguntaConocimiento { get; set; }

        //CARGAR EL DETALLE
        public List<ViewModelEvaluacionEventoDetalle> ListaPreguntaEvaluacionFacilitadorDetalle { get; set; }
        public List<ViewModelEvaluacionEventoDetalle> ListaPreguntaOrganizadorDetalle { get; set; }
        public List<ViewModelEvaluacionEventoDetalle> ListaPreguntaEvaluacionConocimientoDetalle { get; set; }


    }
}
