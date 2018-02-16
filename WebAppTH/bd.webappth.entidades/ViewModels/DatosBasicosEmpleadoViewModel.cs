using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class DatosBasicosEmpleadoViewModel
    {
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres{ get; set; }
        public string Apellidos { get; set; }
        public int IdSexo { get; set; }
        public int IdGenero { get; set; }
        public int IdEstadoCivil { get; set; }
        public int IdTipoSangre { get; set; }
        public int IdNacionalidad { get; set; }
        public int IdEtnia { get; set; }
        public int IdNacionalidadIndigena { get; set; }
        public string CorreoPrivado { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string LugarTrabajo { get; set; }
        public int IdCiudadLugarNacimiento { get; set; }
        public int IdProvinciaLugarSufragio { get; set; }
        public int IdParroquia { get; set; }
        public string CallePrincipal { get; set; }
        public string CalleSecundaria { get; set; }
        public string Referencia { get; set; }
        public string Numero { get; set; }
        public string TelefonoPrivado { get; set; }
        public string TelefonoCasa { get; set; }
        public string Ocupacion { get; set; }
    }
}
