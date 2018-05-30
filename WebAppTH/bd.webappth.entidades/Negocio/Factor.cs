namespace bd.webappth.entidades.Negocio
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Factor
    {
        [Key]
        public int IdFactor { get; set; }
        public decimal? Porciento { get; set; }
    }
}
