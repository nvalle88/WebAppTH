using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.Utils
{
  public static class Mensaje
    {
        public static string NoExisteModulo { get { return "No se ha encontrado el Módulo"; } }
        public static string Excepcion { get { return "Ha ocurrido una Excepción"; } }
        public static string Obligatorio { get { return "Debe introducir datos en el campo"; } }
        public static string FechaRangoMenor { get { return "La fecha inicial no puede ser mayor que la fecha final "; } }
        public static string FechaRangoMayor { get { return "La fecha final no puede ser menor que la fecha inicial "; } }
        public static string ExisteRegistro { get { return "Existe un registro de igual información"; } }
        public static string ExisteEmpleado { get { return "Existe un empleado de igual información"; } }
        public static string Satisfactorio { get { return "La acción se ha realizado satisfactoriamente"; } }
        public static string Error { get { return "Ha ocurrido error inesperado"; } }
        public static string RegistroNoEncontrado { get { return "El registro solicitado no se ha encontrado"; } }
        public static string ModeloInvalido { get { return "El Módelo es inválido"; } }
        public static string BorradoNoSatisfactorio { get { return "No es posible eliminar el registro, existen relaciones que dependen de él"; } }
        public static string NoExistenRegistrosPorAsignar { get { return "No existen Registros por agregar"; } }    
        public static string GenerandoListas { get { return "Las listas se están cargando"; } }
        public static string GuardadoSatisfactorio { get {return "Los datos se han guardado correctamente"; } }
        public static string BorradoSatisfactorio { get { return "El registro se ha eliminado correctamente"; } }
        public static string ErrorFichaEdicion { get { return "Existe una ficha en edición"; } }
        public static string ErrorCargaArchivo { get { return "Se produjo un error al cargar el archivo"; } }
        public static string ErrorServicio { get { return "No se pudo establecer conexión con el servicio"; } }
        public static string SinArchivo { get { return "No existe archivo para descargar"; } }

    }
}
