using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class TipoAccionesPersonalViewModel
    {
        public int IdTipoAccionPersonal { get; set; }
        public string Nombre { get; set; }
        public bool Definitivo { get; set; }
        public bool Dias { get; set; }
        public int YearsMaximo { get; set; }
        public int MesesMaximo { get; set; }
        public int NdiasMinimo { get; set; }
        public int NdiasMaximo { get; set; }
        public bool Horas { get; set; }
        public int NhorasMinimo { get; set; }
        public int NhorasMaximo { get; set; }
        public bool DiasHabiles { get; set; }
        public bool ImputableVacaciones { get; set; }
        public bool ProcesoNomina { get; set; }
        public bool EsResponsableTh { get; set; }
        public string Matriz { get; set; }
        public string Descripcion { get; set; }
        public bool GeneraAccionPersonal { get; set; }
        public bool GenerarMovimientoPersonal { get; set; }
        public bool ModificarDistributivo { get; set; }
        public bool DesactivarCargo { get; set; }
        public bool DesactivarEmpleado { get; set; }
        public bool FinalizaTipoAccionPersonal { get; set; }
        public int? IdTipoAccionPersonalFin { get; set; }

    }
}
