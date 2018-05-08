using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class IndicesOcupacionalesModalidadPartidaViewModel
    {
        public int IdIndiceOcupacionalModalidadPartida { get; set; }
        public int IdIndiceOcupacional { get; set; }
        public int? IdEmpleado { get; set; }
        public int? IdFondoFinanciamiento { get; set; }
        public int? IdTipoNombramiento { get; set; }
        public DateTime Fecha { get; set; }
        public decimal SalarioReal { get; set; }

        public IndiceOcupacionalViewModel IndiceOcupacionalViewModel { get; set; }

    }
}
