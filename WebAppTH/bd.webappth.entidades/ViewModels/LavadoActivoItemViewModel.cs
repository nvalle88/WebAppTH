using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class LavadoActivoItemViewModel
    {
        public int IdLavadoActivoItem { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Seleccione")]
        public bool Valor { get; set; }
        public DateTime Fecha { get; set; }
    }
}
