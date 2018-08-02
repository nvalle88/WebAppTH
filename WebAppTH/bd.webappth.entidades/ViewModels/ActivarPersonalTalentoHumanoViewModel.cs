using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.ViewModels
{
    public class ActivarPersonalTalentoHumanoViewModel
    {


        /* Campos de la tabla ActivarPersonalTalentoHumano */

        public int IdActivarPersonalTalentoHumano { get; set; }
        public int IdDependencia { get; set; }
        public int IdSucursal { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        

        /* Campos de la tabla dependencia */

        public string Nombre { get; set; }

        [Display(Name = "Área geográfica")]
        public string SucursalNombre { get; set; }

        /* Uso: se llena true cuando existe un registro en el año actual */
        public bool Existe { get; set; }

    }
}
