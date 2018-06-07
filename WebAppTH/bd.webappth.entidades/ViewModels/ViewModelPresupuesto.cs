
namespace bd.webappth.entidades.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ViewModelPresupuesto
    {
        public int IdPresupuesto { get; set; }
        [Display(Name = "Numero partida presupuestaria")]
        public string NumeroPartidaPresupuestaria { get; set; }
        [Display(Name = "Valor")]
        public double? Valor { get; set; }
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }
        public int? IdSucursal { get; set; }
        [Display(Name = "Sucursal")]
        public string NombreSucursal { get; set; }

    }
}
