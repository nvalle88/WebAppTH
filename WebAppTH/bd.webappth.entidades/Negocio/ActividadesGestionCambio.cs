namespace bd.webappth.entidades.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class ActividadesGestionCambio
    {
        [Key]
        public int IdActividadesGestionCambio { get; set; }
        public int IdDependencia { get; set; }
        public int IdEmpleado { get; set; }
        public int EstadoActividadesGestionCambio { get; set; }
        public string Tarea { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Avance { get; set; }
        public string Observaciones { get; set; }

        public virtual Dependencia Dependencia { get; set; }
        public virtual Empleado Empleado { get; set; }


    }
}
