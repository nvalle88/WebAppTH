namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
     
    public partial class Relevancia
    {
        [Key]
        public int IdRelevancia { get; set; }
        public string Nombre { get; set; }
        public string ComportamientoObservable { get; set; }
    }
}
