using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelEvaluacionEventoDetalle
    {
            public string Pregunta { get; set; }

            public int? Calificacion { get; set; }
    }
}
