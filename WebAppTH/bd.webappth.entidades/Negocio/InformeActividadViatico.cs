namespace bd.webappth.entidades.Negocio
{
    public partial class InformeActividadViatico
    {
        public int IdInformeActividad { get; set; }
        public int IdSolicitudViatico { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }

        public virtual SolicitudViatico SolicitudViatico { get; set; }
    }
}
