using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class EstadoActividadesGestionCambio
    {
        
        [Key]
        public int IdEstadoActividadesGestionCambio { get; set; }
        public string Nombre { get; set; }
        
    }
}
