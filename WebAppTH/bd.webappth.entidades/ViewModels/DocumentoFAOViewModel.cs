using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class DocumentoFAOViewModel
    {
        public int IdEmpleado { get; set; }
        [DisplayName("Remuneración propuesta")]
        public decimal RemuneracionPropuesta { get; set; }
        [DisplayName("Grado propuesto")]
        public string GradoPropuesto { get; set; }
        [DisplayName("Grupo ocupacional propuesto")]
        public string GrupoOcupacionalPropuesta { get; set; }

        [DisplayName("Nombre")]
        public string nombre { get; set; }
        [DisplayName("Apellido")]
        public string apellido { get; set; }
        [DisplayName("Identificación")]
        public string Identificacion { get; set; }
        [DisplayName("Grupo ocupacional")]
        public string GrupoOcupacional { get; set; }
        public int OpcionMenu { get; set; }
        public string NombreUsuario { get; set; }
        [DisplayName("Partida")]
        public string Partida { get; set; }
        public int idDependencia { get; set; }
        [DisplayName("Remuneración")]
        public decimal Remuneracion { get; set; }
        [DisplayName("Dependencia")]
        public string Dependencia { get; set; }
        public int idsucursal { get; set; }
        public int? estado { get; set; }
        public string Modalidad { get; set; }
        [DisplayName("Tipo nombramiento")]
        public string TipoNombramiento { get; set; }        
        [DisplayName("Institución")]
        public string Institucion { get; set; }
        [DisplayName("Unidad administrativa")]
        public string UnidadAdministrativa { get; set; }
        [DisplayName("Puesto")]
        public string Puesto { get; set; }
        [DisplayName("Nombre Apellido")]
        public string NombreApellido { get; set; }
        [DisplayName("Lugar Trabajo")]
        public string LugarTrabajo { get; set; }
        [Required(ErrorMessage = "Debe introducir {0}")]
        [DisplayName("Mision")]
        public string Mision { get; set; }
        [DisplayName("Actividad")]
        public string actividad { get; set; }
        [DisplayName("Exepcion")]
        public string Exepcion { get; set; }
        public bool InternoMismoProceso { get; set; }
        public bool InternoOtroProceso { get; set; }
        public bool ExternosCiudadania { get; set; }
        public bool ExtPersJuridicasPubNivelNacional { get; set; }
        public int Anio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdFormularioAnalisisOcupacional { get; set; }
        public List<ActividadesAnalisisOcupacional> ListaActividad { get; set; }
        public List<Exepciones> ListaExepcion { get; set; }
        public List<string> ListaActividads { get; set; }
        public List<string> ListaExepciones{ get; set; }
        public List<string> ListActividades { get; set; }
        public List<RolPuesto> ListasRolPUestos { get; set; }
        public List<ManualPuesto> ListasManualPuesto { get; set; }
        public List<string> ListaRolPUesto { get; set; }
        public bool aplicapolitica { get; set; }
        public string Descripcionpuesto { get; set; }
        public bool Cumple { get; set; }
        public bool Revisar { get; set; }
        public int IdManualPuesto { get; set; }
        public int IdManualPuestoActual { get; set; }
        public int IdAdministracionTalentoHumano { get; set; }
        [DisplayName("Puesto Actual")]
        public string PuestoActual { get; set; }
        [DisplayName("Puesto propuesto")]
        public string NuevoPuesto { get; set; }

    }
}
