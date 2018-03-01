using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class ViewModelTrayectoriaLaboral
    {
        
        public int IdTrayectoriaLaboral { get; set; }

        public int IdPersona { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [StringLength(100)]
        public string Empresa { get; set; }

        [StringLength(250)]
        public string PuestoTrabajo { get; set; }

        public string DescripcionFunciones { get; set; }

    }
}
