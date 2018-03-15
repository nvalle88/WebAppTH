using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.Negocio
{
    public partial class FichaMedica
    {
        public FichaMedica()
        {
            AntecedentesFamiliares = new HashSet<AntecedentesFamiliares>();
            AntecedentesLaborales = new HashSet<AntecedentesLaborales>();
            ExamenComplementario = new HashSet<ExamenComplementario>();
        }

        [Display(Name = "Código de ficha médica")]
        public int IdFichaMedica { get; set; }
        public int IdPersona { get; set; }

        [Display(Name = "Fecha de apertura de la historia clínica")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFichaMedica { get; set; }

        [Display(Name = "Antecedentes médicos")]
        public string AntecedenteMedico { get; set; }

        [Display(Name = "Antecedentes quirúrgicos")]
        public string AntecedenteQuirurgico { get; set; }

        [Display(Name = "Alergias")]
        public string Alergias { get; set; }

        [Display(Name = "Uso de medicina diaria")]
        public string UsoMedicinaDiaria { get; set; }

        [Display(Name = "Vacunas")]
        public string Vacunas { get; set; }

        [Display(Name = "Fecha de la última dosis")]
        public DateTime? FechaUltimaDosis { get; set; }

        [Display(Name = "Primera menstruación")]
        public DateTime? PrimeraMenstruacion { get; set; }

        [Display(Name = "Última menstruación")]
        public DateTime? UltimaMenstruacion { get; set; }

        [Display(Name = "Ciclo menstrual")]
        public string CicloMenstrual { get; set; }

        [Display(Name = "Gestas")]
        public int? Gestas { get; set; }

        [Display(Name = "Partos")]
        public int? Partos { get; set; }

        [Display(Name = "Cesarias")]
        public int? Cesarias { get; set; }

        [Display(Name = "Abortos")]
        public int? Abortos { get; set; }

        [Display(Name = "Hijos vivos")]
        public int? HijosVivos { get; set; }

        [Display(Name = "Último paptest")]
        public DateTime? UltimoPapTest { get; set; }

        [Display(Name = "Última mamografía")]
        public DateTime? UltimaMamografia { get; set; }

        [Display(Name = "Anticoncepción")]
        public string Anticoncepcion { get; set; }

        [Display(Name = "Cigarrillo")]
        public bool Cigarrillo { get; set; }

        [Display(Name = "Frecuencia")]
        public string FrecuenciaCigarrillo { get; set; }

        [Display(Name = "Desde (edad en años)")]
        public string CigarrilloDesde { get; set; }

        [Display(Name = "Hasta")]
        public string CigarrilloHasta { get; set; }

        [Display(Name = "Licor")]
        public bool Licor { get; set; }

        [Display(Name = "Frecuencia")]
        public string LicorFrecuencia { get; set; }

        [Display(Name = "Desde (edad en años)")]
        public string LicorDesde { get; set; }

        [Display(Name = "Hasta")]
        public string LicorHasta { get; set; }

        [Display(Name = "Drogas")]
        public bool Drogas { get; set; }

        [Display(Name = "Frecuencia")]
        public string FrecuenciaDrogas { get; set; }

        [Display(Name = "Desde (edad en años)")]
        public string DrogasDesde { get; set; }

        [Display(Name = "Hasta")]
        public string DrogasHasta { get; set; }

        [Display(Name = "Ejercicios")]
        public bool Ejercicios { get; set; }

        [Display(Name = "Frecuencia")]
        public string EjerciciosFrecuencia { get; set; }

        [Display(Name = "Tipo de ejercicios")]
        public string EjerciciosTipo { get; set; }

        [Display(Name = "Tensión arterial")]
        public string TensionArterial { get; set; }

        [Display(Name = "Frecuencia cardiaca")]
        public string FrecuenciaCardiaca { get; set; }

        [Display(Name = "Frecuencia respiratoria")]
        public string FrecuenciaRespiratoria { get; set; }

        [Display(Name = "Talla")]
        public string Talla { get; set; }

        [Display(Name = "Peso")]
        public string Peso { get; set; }

        [Display(Name = "Lateralidad dominante")]
        public int LateralidadDominante { get; set; }

        [Display(Name = "Interpretación")]
        public int Interpretacion { get; set; }

        [Display(Name = "Cabeza")]
        public bool Cabeza { get; set; }

        [Display(Name = "Ojos")]
        public bool Ojos { get; set; }

        [Display(Name = "Oídos")]
        public bool Oidos { get; set; }

        [Display(Name = "Nariz")]
        public bool Nariz { get; set; }

        [Display(Name = "Boca")]
        public bool Boca { get; set; }

        [Display(Name = "Faringe y amígdalas")]
        public bool FaringeAmigdalas { get; set; }

        [Display(Name = "Cuello")]
        public bool Cuello { get; set; }

        [Display(Name = "Corazón")]
        public bool Corazon { get; set; }

        [Display(Name = "Pulmones")]
        public bool Pulmones { get; set; }

        [Display(Name = "Abdomen")]
        public bool Abdomen { get; set; }

        [Display(Name = "Hernias")]
        public bool Hernias { get; set; }

        [Display(Name = "Genitales")]
        public bool Genitales { get; set; }

        [Display(Name = "Extremidades superiores")]
        public bool ExtremidadesSuperiores { get; set; }

        [Display(Name = "Extremidades inferiores")]
        public bool ExtremidadesInferiores { get; set; }

        [Display(Name = "Varices")]
        public bool Varices { get; set; }

        [Display(Name = "Sistema nervioso central")]
        public bool SistemaNerviosoCentral { get; set; }

        [Display(Name = "Piel")]
        public bool Piel { get; set; }

        [Display(Name = "Diagnóstico")]
        public string Diagnostico { get; set; }

        [Display(Name = "¿Sospecha de alguna enfermedad laboral?")]
        public bool SospechaEnfermedadLaboral { get; set; }

        [Display(Name = "Detalle de la enfermedad")]
        public string DetalleEnfermedad { get; set; }

        [Display(Name = "Apto para el cargo")]
        public bool AptoCargo { get; set; }

        [Display(Name = "Recomendaciones")]
        public string Recomendaciones { get; set; }

        [Display(Name = "¿Ha tenido algún accidente de trabajo?")]
        public bool AccidenteTrabajo { get; set; }

        [Display(Name = "Fecha del accidente")]
        public DateTime? FechaAccidente { get; set; }

        [Display(Name = "Empresa")]
        public string EmpresaAccidente { get; set; }

        [Display(Name = "¿Ha tenido alguna enfermedad profesional?")]
        public bool EnfermedadProfesional { get; set; }

        [Display(Name = "Fecha de diagnóstico")]
        public DateTime? FechaDiagnostico { get; set; }

        [Display(Name = "Empresa")]
        public string EmpresaEnfermedad { get; set; }

        [Display(Name = "Detalle el accidente o enfermedad ocupacional")]
        public string DetalleAccidenteEnfermedadOcupacional { get; set; }

        [Display(Name = "Observaciones")]
        public string HabitosObservaciones { get; set; }

        [Display(Name = "Hallazgo")]
        public string CabezaHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string OjosHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string OidosHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string NarizHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string BocaHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string FaringeAmigdalasHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string CuelloHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string CorazonHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string PulmonesHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string AbdomenHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string HerniasHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string GenitalesHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string ExtremidadesSuperioresHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string ExtremidadesInferioresHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string VaricesHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string SistemaNerviosoHallazgos { get; set; }

        [Display(Name = "Hallazgo")]
        public string PielHallazgos { get; set; }


        public int Estado { get; set; }


        public virtual ICollection<AntecedentesFamiliares> AntecedentesFamiliares { get; set; }
        public virtual ICollection<AntecedentesLaborales> AntecedentesLaborales { get; set; }
        public virtual ICollection<ExamenComplementario> ExamenComplementario { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
