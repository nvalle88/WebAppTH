using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.Constantes
{
   public class Constantes
    {
        public const string NivelProfesional = "Profesional";
        public const string NivelNoProfesional = "No profesional";

        /// <summary>
        /// Constantes para la session de ficha de empleado
        /// </summary>
        public const string idEmpleadoSession = "idEmpleado";
        public const string idPersonaSession = "idPersona";

        /// <summary>
        /// Constantes para la session de ficha de los candidatos
        /// </summary>
        public const string idCandidatoSession = "idCandidatoSession";
        public const string idCandidatoPersonaSession = "idCandidatoPersonaSession";
        public const string idEvaluadorSession = "idEvaluadorSession";
        public const string idEval011Session = "idEval011Session";

        /// <summary>
        /// Constantes para la session de CadidatoConcurso
        /// </summary>
        public const string idCandidatoConcursoSession = "idCandidatoConcursoSession";
        public const string idDependeciaConcursoSession = "idDependeciaConcursoSession";
        public const string idParidaFaseConcursoSession = "idParidaFaseConcursoSession";

        /// <summary>
        /// Constantes para la session de CadidatoConcurso
        /// </summary>
        public const string idIndiceOcupacionalSession = "idIndiceOcupacionalSession";


        /// <summary>
        /// Constantes para las secciones de la configutración de la nómina 
        /// Conceptos 
        /// </summary>
        public const string idConceptoNominaSession = "idConceptoNominaSession";
        public const string CodigoConceptoNominaSession = "CodigoConceptoNominaSession";
        public const string DescripcionConceptoNominaSession = "DescripcionConceptoNominaSession";

        /// <summary>
        /// Variables de sección para la configuración de tabla del sri
        /// </summary>
        public const string  IdSriSession= "IdSriSession";
        public const string DescripcionSriSession = "DescripcionSriSession";

        /// <summary>
        /// Variables de sección para los gastos personales de cada empleado
        /// </summary>
        public const string IdEmpleadoGastoPersonal = "IdEmpleadoGastoPersonal";
        public const string NombreEmpleadoGastoPersonal = "NombreEmpleadoGastoPersonal";
        public const string IdentificacionGastoPersonal = "IdentificacionGastoPersonal";


        /// <summary>
        /// Variables de sección para informe viaticos
        /// </summary>
        public const string IdItinerario = "IdItinerario";
        public const string IdSolicitudtinerario = "IdSolicitudtinerario";


        /// <summary>
        /// Variables de sección para calculo de Nomina 
        /// </summary>
        public const string IdCalculoNominaSession = "IdCalculoNominaSession";
    }
}
