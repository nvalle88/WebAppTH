using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DatosBasicosPersonaViewModel
    {
        public int IdPersona { get; set; }

        [Display(Name = "Nombres y apellidos")]
        public string NombresApellidos { get; set; }

        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }

        [Display(Name = "Lugar de nacimiento")]
        public string LugarNacimiento { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public string FechaNacimiento { get; set; }

        [Display(Name = "Dirección domiciliaria")]
        public string DireccionDomiciliaria { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Edad")]
        public string Edad { get; set; }

        [Display(Name = "Género")]
        public string Genero { get; set; }

        [Display(Name = "Nivel educativo")]
        public string NivelEducativo { get; set; }

        [Display(Name = "Estado civil")]
        public string EstadoCivil { get; set; }

        [Display(Name = "Profesión")]
        public string Profesion { get; set; }

        [Display(Name = "Número de hijos")]
        public int NumeroHijos { get; set; }

        [Display(Name = "Etnia")]
        public string Etnia { get; set; }

        [Display(Name = "Condición especial")]
        public bool CondicionEspecial { get; set; }

        [Display(Name = "Tipo de discapacidad")]
        public string TipoDiscapacidad { get; set; }

        [Display(Name = "N° Conadis")]
        public string Conadis { get; set; }

        [Display(Name = "Porcentaje")]
        public string Porcentaje { get; set; }

        [Display(Name = "Nombre del cargo de trabajo")]
        public string NombreCargoTrabajo { get; set; }

        [Display(Name = "Descripción del puesto de trabajo")]
        public string DescripcionPuestoTrabajo { get; set; }

        [Display(Name = "Sede de trabajo")]
        public string SedeTrabajo { get; set; }

        [Display(Name = "Contacto en caso de emergencias")]
        public string ContactoEmergencias { get; set; }

        [Display(Name = "Parentesco")]
        public string Parentesco { get; set; }

        [Display(Name = "Teléfono")]
        public string ParienteTelefono { get; set; }

        [Display(Name = "Tipo de sangre")]
        public string SangreTipo { get; set; }

        [Display(Name = "fecha de ingreso a la institución")]
        public DateTime FechaIngreso { get; set; }
    }
}
