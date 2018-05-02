using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class TipoAccionesPersonalViewModel
    {
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
