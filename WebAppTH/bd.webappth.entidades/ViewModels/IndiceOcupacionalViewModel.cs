using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
  public  class IndiceOcupacionalViewModel
    {
        public int IdIndiceOcupacional { get; set; }
        [DisplayName("Dependencia")]
        public string Dependencia { get; set; }
        [DisplayName("Denominación del puesto")]
        public string ManualPuesto { get; set; }
        [DisplayName("Grado")]
        public int Grado { get; set; }
        [DisplayName("Misión")]
        public string Mision { get; set; }
        [DisplayName("Relaciones internas y externas")]
        public string RelacionesInternasExternas { get; set; }
        [DisplayName("Rol")]
        public string RolPuesto { get; set; }
        [DisplayName("Grupo ocupacional")]
        public string EscalaGrado { get; set; }
        [DisplayName("Remuneración")]
        public decimal Remuneracion { get; set; }
        [DisplayName("Modalidad partida")]
        public string ModalidadPartida { get; set; }
        [DisplayName("Partida general")]
        public int PartidaGeneral { get; set; }
        [DisplayName("Partida individual")]
        public string PartidaIndividual { get; set; }
        [DisplayName("Ámbito")]
        public string Ambito { get; set; }
        [DisplayName("Nivel")]
        public string Nivel { get; set; }
        public string Sucursal { get; set; }
        [DisplayName("Codigo dependencia")]
        public string CodigoDepencia { get; set; }

        public int OpcionMenu { get; set; }
    }
}
