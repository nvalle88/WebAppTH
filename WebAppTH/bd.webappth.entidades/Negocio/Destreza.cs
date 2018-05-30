namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Destreza
    {
        [Key]
        public int IdDestreza { get; set; }
        public string Nombre { get; set; }
    }
}
