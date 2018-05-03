using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class TipoAccionesPersonalViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar el {0} ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el Tipo de movimiento")]
        public int IdTipoAccionPersonal { get; set; }

        public string Nombre { get; set; }

        public int NDiasMinimo { get; set; }

        public int NDiasMaximo { get; set; }

        public int NHorasMinimo { get; set; }

        public int NHorasMaximo { get; set; }

        public bool DiasHabiles { get; set; }

        public bool ImputableVacaciones { get; set; }

        public bool ProcesoNomina { get; set; }

        public bool EsResponsableTH { get; set; }

        public string Matriz { get; set; }

        public string Descripcion { get; set; }

        public bool GeneraAccionPersonal { get; set; }

        public bool ModificaDistributivo { get; set; }

        public int IdEstadoTipoAccionPersonal { get; set; }
    }
}
