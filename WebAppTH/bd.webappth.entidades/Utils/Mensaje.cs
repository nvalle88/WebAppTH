﻿using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.Utils
{
    public static class Mensaje
    {

        public static string FormulaNominaInvalida { get { return "La fórmula tiene algún error.Por favor verifique nuevamente."; } }

        public static string FaltaIngresoDatos { get { return "Debe completar el formulario"; } }
        public static string NoExisteModulo { get { return "No se ha encontrado el Módulo"; } }
        public static string Excepcion { get { return "Ha ocurrido una Excepción"; } }
        public static string Obligatorio { get { return "Debe introducir datos en el campo"; } }
        public static string FechaRangoMenor { get { return "La fecha inicial no puede ser mayor que la fecha final "; } }
        public static string FechaRangoMayor { get { return "La fecha final no puede ser menor que la fecha inicial "; } }
        public static string ExisteRegistro { get { return "Existe un registro de igual información"; } }
        public static string ExisteEmpleado { get { return "Existe un empleado de igual información"; } }
        public static string Satisfactorio { get { return "La acción se ha realizado satisfactoriamente"; } }
        public static string RegistroNoEncontrado { get { return "El registro solicitado no se ha encontrado"; } }
        public static string ModeloInvalido { get { return "El Módelo es inválido"; } }
        public static string BorradoNoSatisfactorio { get { return "No es posible eliminar el registro, existen relaciones que dependen de él"; } }
        public static string NoExistenRegistrosPorAsignar { get { return "No existen Registros por agregar"; } }
        public static string GenerandoListas { get { return "Las listas se están cargando"; } }
        public static string GuardadoSatisfactorio { get { return "Los datos se han guardado correctamente"; } }
        public static string BorradoSatisfactorio { get { return "El registro se ha eliminado correctamente"; } }
        public static string ErrorFichaEdicion { get { return "Existe una ficha en edición"; } }
        public static string ErrorCargaArchivo { get { return "Se produjo un error al cargar el archivo"; } }
        public static string ErrorServicio { get { return "No se pudo establecer conexión con el servicio"; } }
        public static string CorregirFormulario { get { return "Corregir la información"; } }
        public static string SinArchivo { get { return "No existe archivo para descargar"; } }
        public static string RegistroEditado { get { return "El registro se ha editado corectamente"; } }

        public static string RegistroNoExiste { get { return "El registro que desea editar no existe."; } }
        public static string ErrorCrear { get { return "Ha ocurrido un error al crear el registro."; } }
        public static string ErrorEditar { get { return "Ha ocurrido un error al editar el registro."; } }
        public static string ErrorEliminar { get { return "Ha ocurrido un error al eliminar el registro."; } }
        public static string ErrorListado { get { return "Ha ocurrido un error al cargar el listado."; } }
        public static string ErrorCargarDatos { get { return "Ha ocurrido un error al cargar los datos."; } }
        public static string Informacion { get { return "Información"; } }
        public static string Error { get { return "Error"; } }
        public static string Aviso { get { return "Aviso"; } }
        public static string Success { get { return "Success"; } }

        public static string ErrorActivar { get { return "Ha ocurrido un error al activar el registro."; } }
        public static string ErrorDesactivar { get { return "Ha ocurrido un error al desactivar el registro."; } }

        public static string SeleccioneIndice { get { return "Debe seleccionar un índice en: situación propuesta."; } }
        public static string SeleccioneSolicitudPlanificacionVacaciones { get { return "Seleccione una solicitud de planificacion"; } }
        public static string MotivoSolicitudVacacionNoPlanificada { get { return "Escriba un motivo"; } }

        public static string ConceptoNoExiste { get { return "El concepto no existe."; } }
        public static string EmpleadoNoExiste { get { return "Identificación del empleado no existe."; } }
        public static string ConceptoEmpleadoNoExiste { get { return "El concepto y la Identificación del empleado no existen."; } }

        public static object SeleccionarFichero { get { return "Debe seleccionar un fichero..."; } }

        public static object ReportadoConErrores { get { return "Verifique la información del los reportados cargados ya que existen errores en su información, la información con errores no fue guardada..."; } }
        public static object ReportadoNoCumpleFormato { get { return "Verifique el formato del archivo seleccionado,Nota: Debe seleccionar un archivo Excel(.xlsx).El cual debe contener el orden de las columnas de la siguiente distribución...1:Código del concepto, 2:Identificación del empleado, 3:Nombre y Apellidos del empleado, 4:Cantidad, 5:Importe"; } }

        public static string NoExistenRegistros { get { return "No existen registros para mostrar"; } }

        public static string ErrorFechaDesdeHasta { get { return "La fecha desde debe ser menor que la fecha hasta."; } }
        public static string AccesoNoAutorizado { get { return "No tiene los permisos necesarios para acceder a este sitio"; } }

        public static string SessionCaducada { get { return "La sesión ha caducado, ha sido devuelto a la página principal del proceso"; } }

        public static string EscogerEmpleadoReemplazo { get { return "Seleccione un reemplazo"; } }

        public static object NoAsignadoDistrivutibo { get { return "El empleado seleccionado no está asignado al distributivo, debe asignar el empleado al distributivo para editar su ficha..."; } }

        public static object NoProcesarSolicitud { get { return "No se ha podido porcesar la solicitud realizada "; } }

        public static object AgregandoEmpleadoDistrivutibo { get { return "El empleado se ha insertado satisfactoriamente, complete el Formulario y presione Guardar para agregar el empleado al distributivo, en caso contrario Cancelar."; } }
        

        public static object HorasExtrasNoCumpleFormato { get { return "Verifique el formato del archivo seleccionado,Nota: Debe seleccionar un archivo Excel(.xlsx).El cual debe contener el orden de las columnas de la siguiente distribución...1:Identificación del empleado, 2:Cantidad de horas, 3:1 si son horas extraordinarias ,0 si no son horas extraordinarias"; } }

        public static object DiasLaboradosNoCumpleFormato { get { return "Verifique el formato del archivo seleccionado,Nota: Debe seleccionar un archivo Excel(.xlsx).El cual debe contener el orden de las columnas de la siguiente distribución...1:Identificación del empleado, 2:Cantidad de días laborados"; } }

        public static object HorasExtrasConErrores { get { return "Verifique la información de las horas extras cargadas ya que existen errores en su información, la información con errores no fue guardada..."; } }
        public static object DiasLaboradosConErrores { get { return "Verifique la información de los días laborados cargados ya que existen errores en su información, la información con errores no fue guardada..."; } }

        public static object RequeridoFondoFinanciamiento { get { return "No ha seleccionado un fondo de financiamiento"; } }
    }
}
