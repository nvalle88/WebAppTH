using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DatosBasicosEmpleadoSinRequiredViewModel 
    {
        public int IdEmpleado { get; set; }

        
        [Display(Name = "Tipo de identificación")]
        public int IdTipoIdentificacion { get; set; }
        
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }
        
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }
        
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }
        
        [Display(Name = "Sexo")]
        public int IdSexo { get; set; }
        
        [Display(Name = "Género")]
        public int IdGenero { get; set; }
        
        [Display(Name = "Estado Cívil")]
        public int IdEstadoCivil { get; set; }
        
        [Display(Name = "Tipo de sangre")]
        public int IdTipoSangre { get; set; }
        
        [Display(Name = "Nacionalidad")]
        public int IdNacionalidad { get; set; }
        
        [Display(Name = "Etnia")]
        public int IdEtnia { get; set; }

        public int? IdNacionalidadIndigena { get; set; }
        
        [Display(Name = "Correo privado")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        public string CorreoPrivado { get; set; }
        
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }
        
        [Display(Name = "Lugar de trabajo")]
        public string LugarTrabajo { get; set; }
        
        [Display(Name = "País")]
        public int IdPaisLugarNacimiento { get; set; }
        
        [Display(Name = "Ciudad")]
        public int IdCiudadLugarNacimiento { get; set; }
        
        [Display(Name = "País")]
        public int IdPaisLugarSufragio { get; set; }
        
        [Display(Name = "Provincia")]
        public int IdProvinciaLugarSufragio { get; set; }
        
        [Display(Name = "País")]
        public int IdPaisLugarPersona { get; set; }
        
        [Display(Name = "Provincia")]
        public int IdProvinciaLugarPersona { get; set; }
        
        [Display(Name = "Ciudad")]
        public int IdCiudadLugarPersona { get; set; }
        
        [Display(Name = "Parroquia")]
        public int IdParroquia { get; set; }
        
        [Display(Name = "Calle principal")]
        public string CallePrincipal { get; set; }
        
        [Display(Name = "Calle secundaria")]
        public string CalleSecundaria { get; set; }
        
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }
        
        [Display(Name = "Número de casa")]
        public string Numero { get; set; }
        
        [Display(Name = "Teléfono privado")]
        public string TelefonoPrivado { get; set; }
        
        [Display(Name = "Teléfono de casa")]
        public string TelefonoCasa { get; set; }
        
        [Display(Name = "Ocupación")]
        public string Ocupacion { get; set; }

        
    }
}
