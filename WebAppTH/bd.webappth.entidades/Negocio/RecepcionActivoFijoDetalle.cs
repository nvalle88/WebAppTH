using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
    public class RecepcionActivoFijoDetalle
    {
        [Key]
        public int IdRecepcionActivoFijoDetalle { get; set; }
        public int IdRecepcionActivoFijo { get; set; }
        public int IdActivoFijo { get; set; }
        public int IdEstado { get; set; }
        public string NumeroPoliza { get; set; }

        public virtual Estado Estado { get; set; }
    }
}
