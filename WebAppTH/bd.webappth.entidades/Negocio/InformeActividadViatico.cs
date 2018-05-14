namespace bd.webappth.entidades.Negocio
{
    public partial class InformeActividadViatico
    {
        public int IdInformeActividad { get; set; }
        public int? IdItinerarioViatico { get; set; }
        public string Descripcion { get; set; }

        public virtual ItinerarioViatico ItinerarioViatico { get; set; }
    }
}
