using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class VacacionesEmpleadoViewModel
    {
        public int IdVacaciones { get; set; }
        public int PeriodoFiscal { get; set; }
        public int IdEmpleado { get; set; }
        public int VacacionesGozadas { get; set; }
        public int VacacionesNoGozadas { get; set; }
    }
}
