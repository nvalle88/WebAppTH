using System;
using System.Collections.Generic;

namespace bd.webappth.entidades.Negocio
{
    public partial class DetalleReliquidacionViatico
    {
        public int IdDetalleReliquidacionViatico { get; set; }
        public int IdReliquidacionViatico { get; set; }
        public int? IdTipoTransporte { get; set; }
        public int? IdItemViatico { get; set; }
        public string NombreTransporte { get; set; }
        public int? IdCiudadOrigen { get; set; }
        public int? IdCiudadDestino { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public TimeSpan? HoraLlegada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public string Descripcion { get; set; }
        public decimal? ValorEstimado { get; set; }

        public virtual Ciudad CiudadDestino { get; set; }
        public virtual Ciudad CiudadOrigen { get; set; }
        public virtual ItemViatico ItemViatico { get; set; }
        public virtual ReliquidacionViatico ReliquidacionViatico { get; set; }
        public virtual TipoTransporte TipoTransporte { get; set; }
    }
}
