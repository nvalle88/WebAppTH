using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class EmpleadoListaEval001ViewModel
    {
        public EmpleadoEvaluadoViewModel EmpleadoEvaluado { get; set; }
        
        public Empleado EmpleadoEvaluador { get; set; }

        public List<Eval001> ListaEval001 { get; set; }

    }
}
