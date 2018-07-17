using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ObjectTransfer;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace bd.webappth.web.Controllers.MVC
{
    public class MigracionCapacitacionesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IApiServicio apiServicio;
        private readonly IUploadFileService uploadFileService;

        public MigracionCapacitacionesController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment, IUploadFileService uploadFileService)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;
            this.uploadFileService = uploadFileService;

        }

        public async Task<FileResult> Download(string id)
        {

            try
            {
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, string.Format("{0}/{1}.{2}", "DocumentoCapacitacion/Reportados", id, "xlsx"));
                var file = new FileStream(targetDirectory, FileMode.Open);
                byte[] data;
                using (var br = new BinaryReader(file))
                    data = br.ReadBytes((int)file.Length);
                var fileName = $"{id}.xlsx";

                return File(data, "application/xlsx", fileName);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<IActionResult> ReportadoCapacitacion(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (HttpContext.Session.GetInt32(Constantes.IdCapacitacionSession) != Convert.ToInt32(id))
                    {
                        HttpContext.Session.SetInt32(Constantes.IdCapacitacionSession, Convert.ToInt32(id));
                    }
                    var Capacitacion = new PlanCapacitacion { IdPlanCapacitacion = ObtenerIdCapacitacion().IdGestionPlanCapacitacion };
                    return View(Capacitacion);
                }

                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        private async Task<FileInfo> SubirFichero(List<IFormFile> files)
        {
            byte[] data;
            using (var br = new BinaryReader(files[0].OpenReadStream()))
                data = br.ReadBytes((int)files[0].OpenReadStream().Length);
            string sFileName = files[0].FileName;
            await uploadFileService.UploadFile(data, "DocumentoCapacitacion/Reportados", Convert.ToString(ObtenerIdCapacitacion().IdGestionPlanCapacitacion), ".xlsx");
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, string.Format("{0}/{1}.{2}", "DocumentoCapacitacion/Reportados", Convert.ToString(ObtenerIdCapacitacion().IdGestionPlanCapacitacion), "xlsx")));
            return file;
        }


        private async Task<List<PlanCapacitacion>> LeerExcel(FileInfo file)
        {
            try
            {
                var lista = new List<PlanCapacitacion>();
                var listaSalida = new List<PlanCapacitacion>();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    var idGestionCapacitaciones = ObtenerIdCapacitacion().IdGestionPlanCapacitacion;
                    DateTime? Fecha = new DateTime();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var NumeroPartida = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString();
                        var Institucion = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString();
                        var Pais = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString();
                        var Provincia = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString();
                        var Ciudad = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString();
                        var NivelDesconcentracion = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString();
                        var UnidadAdministrativa = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString();
                        var Cedula = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString();
                        var NombreApellido = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString();
                        var Sexo = worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString();
                        var GrupoOcupacional = worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString();
                        var DenominacionPuesto = worksheet.Cells[row, 12].Value == null ? "" : worksheet.Cells[row, 12].Value.ToString();
                        var RegimenLaboral = worksheet.Cells[row, 13].Value == null ? "" : worksheet.Cells[row, 13].Value.ToString();
                        var ModalidadLaboral = worksheet.Cells[row, 14].Value == null ? "" : worksheet.Cells[row, 14].Value.ToString();
                        var TemaCapacitacion = worksheet.Cells[row, 15].Value == null ? "" : worksheet.Cells[row, 15].Value.ToString();
                        var ClasificacionTema = worksheet.Cells[row, 16].Value == null ? "" : worksheet.Cells[row, 16].Value.ToString();
                        var ProductoFinal = worksheet.Cells[row, 17].Value == null ? "" : worksheet.Cells[row, 17].Value.ToString();
                        var ModalidadPlanificada = worksheet.Cells[row, 18].Value == null ? "" : worksheet.Cells[row, 18].Value.ToString();
                        var Duracion = worksheet.Cells[row, 19].Value == null ? "0" : worksheet.Cells[row, 19].Value.ToString();
                        var Presupuesto = worksheet.Cells[row, 20].Value == null ? "0" : worksheet.Cells[row, 20].Value.ToString();
                        Fecha = Convert.ToDateTime(worksheet.Cells[row, 21].Value == null ? null : worksheet.Cells[row, 21].Value).Date;
                        //var TipoPlanificacion = worksheet.Cells[row, 22].Value == null ? "" : worksheet.Cells[row, 22].Value.ToString();

                        //var cantidadStr = worksheet.Cells[row, 4].Value == null ? Convert.ToString(0.0) : worksheet.Cells[row, 4].Value.ToString() ;
                        //var importeStr =worksheet.Cells[row, 5].Value == null ? Convert.ToString(0.0) :worksheet.Cells[row, 5].Value.ToString();

                        //cantidadStr = cantidadStr.Replace(",", ",");
                        //importeStr = importeStr.Replace(",", ",");
                        var DuracionCapacitacion = Convert.ToInt32(Duracion);
                        var ValorPresupuesto = Convert.ToDecimal(Presupuesto);

                        lista.Add(new PlanCapacitacion
                        {
                            IdGestionPlanCapacitacion = idGestionCapacitaciones,
                            NumeroPartidaPresupuestaria = NumeroPartida,
                            Institucion = Institucion,
                            Pais = Pais,
                            Provincia = Provincia,
                            NombreCiudad = Ciudad,
                            NivelDesconcentracion = NivelDesconcentracion,
                            UnidadAdministrativa = UnidadAdministrativa,
                            Cedula = Cedula,
                            ApellidoNombre = NombreApellido,
                            Sexo = Sexo,
                            GrupoOcupacional = GrupoOcupacional,
                            DenominacionPuesto = DenominacionPuesto,
                            RegimenLaboral = RegimenLaboral,
                            ModalidadLaboral = ModalidadLaboral,
                            TemaCapacitacion = TemaCapacitacion,
                            ClasificacionTema = ClasificacionTema,
                            ProductoFinal = ProductoFinal,
                            Modalidad = ModalidadPlanificada,
                            Duracion = DuracionCapacitacion,
                            PresupuestoIndividual = ValorPresupuesto,
                            FechaCapacitacionPlanificada = Fecha,
                            //TipoCapacitacion = TipoPlanificacion,
                            Estado = 1
                        });

                    }
                    listaSalida = await apiServicio.Listar<PlanCapacitacion>(lista, new Uri(WebApp.BaseAddress),
                                               "api/MigracionCapacitaciones/VerificarExcel");

                }
                return listaSalida;
                // return lista;


            }
            catch (Exception ex)
            {
                return new List<PlanCapacitacion>();
            }
        }

        public async Task<IActionResult> LimpiarReportados(string id)
        {
            try
            {

                if (!string.IsNullOrEmpty(id))
                {

                    var envia = new GestionPlanCapacitacion { IdGestionPlanCapacitacion = Convert.ToInt32(id) };
                    var response = await apiServicio.ObtenerElementoAsync1<Response>(envia, new Uri(WebApp.BaseAddress)
                                                                              , "api/MigracionCapacitaciones/LimpiarReportados");
                    if (response.IsSuccess)
                    {
                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.Excepcion}");
            }
        }

        public async Task<IActionResult> MostrarExcelBase(string id)
        {
            try
            {

                if (!string.IsNullOrEmpty(id))
                {

                    if (HttpContext.Session.GetInt32(Constantes.IdCapacitacionSession) != Convert.ToInt32(id))
                    {
                        HttpContext.Session.SetInt32(Constantes.IdCapacitacionSession, Convert.ToInt32(id));
                    }
                    var envia = new GestionPlanCapacitacion { IdGestionPlanCapacitacion = Convert.ToInt32(id) };
                    var lista = await apiServicio.Listar<PlanCapacitacion>(envia, new Uri(WebApp.BaseAddress)
                                                                              , "api/MigracionCapacitaciones/ListarReportados");
                    if (lista.Count == 0)
                    {
                        return this.Redireccionar($"{Mensaje.Error}|{Mensaje.NoExistenRegistros}");
                    }
                    return View(lista);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostrarExcel(PlanCapacitacion planCapacitacion, List<IFormFile> files)
        {
            try
            {
                if (files.Count <= 0)
                {
                    return this.Redireccionar("MigracionCapacitaciones", "ReportadoNomina", new { id = Convert.ToString(ObtenerIdCapacitacion().IdGestionPlanCapacitacion) }, $"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
                }


                var file = await SubirFichero(files);
                var lista = await LeerExcel(file);
                if (lista.Count == 0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ReportadoNoCumpleFormato}|{"45000"}";
                    return View(lista);
                }
                var listaSalvar = lista.Where(x => x.Valido == true).ToList();
                //var listaSalvar = lista.Where(x => x.IdPlanCapacitacion == true).ToList();
                var reportadoRequest = new Response();
                if (listaSalvar.Count > 0)
                {
                    reportadoRequest = await apiServicio.InsertarAsync<Response>(listaSalvar, new Uri(WebApp.BaseAddress),
                              "api/MigracionCapacitaciones/InsertarReportadoPlanificacion");
                }
                var listaErrores = lista.Where(x => x.Valido == false).ToList();
                if (listaErrores.Count > 0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{Mensaje.ReportadoConErrores}|{"12000"}";
                }
                else
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Success}|{Mensaje.Satisfactorio}";
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
            }

        }


        public async Task<IActionResult> Create(string mensaje)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GestionPlanCapacitacion gestionPlanCapacitacion)
        {
            Response response = new Response();

            try
            {
                response = await apiServicio.InsertarAsync(gestionPlanCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GestionCapacitaciones/InsertarGestionCapacitaciones");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                return View(gestionPlanCapacitacion);

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }
        public async Task<IActionResult> CreatePlanCapacitacion(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var datos = new PlanCapacitacion
                    {

                        IdGestionPlanCapacitacion = Convert.ToInt32(id)
                    };
                    await cargarCombox();
                    return View(datos);
                }

                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlanCapacitacion(PlanCapacitacion planCapacitacion)
        {
            var respuesta = new Response();
            try
            {
                respuesta = await apiServicio.InsertarAsync<Response>(planCapacitacion, new Uri(WebApp.BaseAddress),
                                                              "api/MigracionCapacitaciones/InsertarPlanCapacitacion");
                if (respuesta.IsSuccess)
                {
                    //await cargarCombox();
                    //var vista = JsonConvert.DeserializeObject<PlanCapacitacion>(respuesta.Resultado.ToString());
                    //return View(vista);
                    return RedirectToAction("MostrarExcelBase", new { id = planCapacitacion.IdGestionPlanCapacitacion });
                }


                return this.Redireccionar($"{Mensaje.Error}|{respuesta.Message}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{respuesta.Message}");
            }
        }

        public async Task<JsonResult> ObterDatosEmpleado(int idEmpleado)
        {
            var envia = new PlanCapacitacion { IdEmpleado = idEmpleado };

           var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(envia, new Uri(WebApp.BaseAddress),
                                                                  "api/MigracionCapacitaciones/ObtenerDatosEmpelado");
            
                return Json(respuesta.Resultado);
            
            
        }
        public async Task<IActionResult> EditPlanCapacitacion(string id)
        {
            var respuesta = new Response();
            try
            {

                if (!string.IsNullOrEmpty(id))
                {
                    var envia = new PlanCapacitacion { IdPlanCapacitacion = Convert.ToInt32(id) };
                    respuesta = await apiServicio.ObtenerElementoAsync1<Response>(envia, new Uri(WebApp.BaseAddress),
                                                                  "api/MigracionCapacitaciones/ObtenerDatosPlanCapacitaciones");
                    if (respuesta.IsSuccess)
                    {
                        await cargarCombox();
                        var vista = JsonConvert.DeserializeObject<PlanCapacitacion>(respuesta.Resultado.ToString());
                        return View(vista);
                    }
                }

                return this.Redireccionar($"{Mensaje.Error}|{respuesta.Message}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{respuesta.Message}");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlanCapacitacion(PlanCapacitacion planCapacitacion)
        {
            var respuesta = new Response();
            try
            {
                respuesta = await apiServicio.EditarAsync<Response>(planCapacitacion, new Uri(WebApp.BaseAddress),
                                                              "api/MigracionCapacitaciones/EditarPlanCapacitacion");
                if (respuesta.IsSuccess)
                {
                    //await cargarCombox();
                    //var vista = JsonConvert.DeserializeObject<PlanCapacitacion>(respuesta.Resultado.ToString());
                    //return View(vista);
                    return RedirectToAction("MostrarExcelBase",new {id = planCapacitacion.IdGestionPlanCapacitacion });
                }


                return this.Redireccionar($"{Mensaje.Error}|{respuesta.Message}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{respuesta.Message}");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var envia = new GestionPlanCapacitacion { IdGestionPlanCapacitacion = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(envia, new Uri(WebApp.BaseAddress),
                                                                  "api/GestionCapacitaciones/ObtenerCalculoNomina");
                    if (respuesta.IsSuccess)
                    {

                        var vista = JsonConvert.DeserializeObject<GestionPlanCapacitacion>(respuesta.Resultado.ToString());
                        return View(vista);
                    }
                }

                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }


        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var envia = new GestionPlanCapacitacion { IdGestionPlanCapacitacion = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(envia, new Uri(WebApp.BaseAddress)
                                                               , "api/GestionCapacitaciones/EliminarGestionCapacitacion");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }
        public async Task<IActionResult> Desactivar(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var envia = new PlanCapacitacion { IdPlanCapacitacion = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(envia, new Uri(WebApp.BaseAddress)
                                                               , "api/MigracionCapacitaciones/Desactivar");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GestionPlanCapacitacion gestionPlanCapacitacion)
        {
            Response response = new Response();
            try
            {

                response = await apiServicio.EditarAsync<Response>(gestionPlanCapacitacion, new Uri(WebApp.BaseAddress),
                                                             "api/GestionCapacitaciones/EditarGestionCapacitaciones");

                if (response.IsSuccess)
                {

                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                return View(gestionPlanCapacitacion);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            try
            {

                var lista = await apiServicio.Listar<GestionPlanCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                     , "api/GestionCapacitaciones/ListarGestionPlanCapacitaciones");
                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }
        

        public GestionPlanCapacitacion ObtenerIdCapacitacion()
        {
            var gastoPersonal = new GestionPlanCapacitacion
            {
                IdGestionPlanCapacitacion = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.IdCapacitacionSession)),
            };
            return gastoPersonal;
        }
        public async Task cargarCombox()
        {
            ViewData["IdProveedorCapacitaciones"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionProveedor>(new Uri(WebApp.BaseAddress), "api/CapacitacionesProveedores/ListarCapacitacionesProveedores"), "IdCapacitacionProveedor", "Nombre");
            ViewData["IdCiudad"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdPresupuesto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ViewModelPresupuesto>(new Uri(WebApp.BaseAddress), "api/Presupuesto/ListarPresupuestoCapacitaciones"), "IdPresupuesto", "NumeroPartidaPresupuestaria");
            ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosActivos"), "IdEmpleado", "NombreApellido");
            ViewData["TipoCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress), "api/GeneralCapacitacion/ListarGeneralCapacitacionTipoCapacitacion"), "IdGeneralCapacitacion", "Nombre");
            ViewData["EstadoEvento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress), "api/GeneralCapacitacion/ListarGeneralCapacitacionEstadoEvento"), "IdGeneralCapacitacion", "Nombre");
            ViewData["AmbitoCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress), "api/GeneralCapacitacion/ListarGeneralCapacitacionAmbitoCapacitacion"), "IdGeneralCapacitacion", "Nombre");
            ViewData["NombreEvento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress), "api/GeneralCapacitacion/ListarGeneralCapacitacionNombreEvento"), "IdGeneralCapacitacion", "Nombre");
            ViewData["TipoEvento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress), "api/GeneralCapacitacion/ListarGeneralCapacitacionTipoEvento"), "IdGeneralCapacitacion", "Nombre");
            ViewData["TipoEvaluacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress), "api/GeneralCapacitacion/ListarGeneralCapacitacionTipoEvaluacion"), "IdGeneralCapacitacion", "Nombre");
        }
    }
}