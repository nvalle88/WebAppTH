using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class Ejemplo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime Fecha { get; set; }
        public int? Edad { get; set; }
    }
}
