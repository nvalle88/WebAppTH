using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public class CalculoNominaController: Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IApiServicio apiServicio;
        private readonly IUploadFileService uploadFileService;

        public CalculoNominaController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment, IUploadFileService uploadFileService)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;
            this.uploadFileService = uploadFileService;

        }


        public async Task<FileResult> Download(string id)
        {
            
            try
            {
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, string.Format("{0}/{1}.{2}", "DocumentoNomina/Reportados", id, "xlsx"));
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

        public async Task<IActionResult> CalcularDetalleNomina(int id)
        {

            var calculoNomina = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };
            var response = await apiServicio.ObtenerElementoAsync1<Response>(calculoNomina, new Uri(WebApp.BaseAddress)
                                                                      , "api/CalculoNomina/CalcularDetalleNomina");
            return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
        }

        public async Task<IActionResult> ReportadoNomina()
        {
            try
            {
             return View(await ObtenerCalculoNomina());

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> HorasExtras()
        {
            try
            {
                return View(await ObtenerCalculoNomina());

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> DiasLaborados()
        {
            try
            {
                return View();

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostrarDiasLaborados( List<IFormFile> files)
        {
            try
            {
                if (files.Count <= 0)
                {
                    return this.Redireccionar("CalculoNomina", "DiasLaborados", new { id = Convert.ToString(ObtenerCalculoNomina().Result.IdCalculoNomina) }, $"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
                }
                var file = await SubirFichero(files, "DocumentoNomina/DiasLaborados");
                var lista = await LeerExcelDiasLaborados(file);
                if (lista.Count == 0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.DiasLaboradosNoCumpleFormato}|{"45000"}";
                    return View(lista);
                }
                var listaSalvar = lista.Where(x => x.Valido == true).ToList();
                var reportadoRequest = new Response();
                if (listaSalvar.Count > 0)
                {
                    reportadoRequest = await apiServicio.InsertarAsync<Response>(listaSalvar, new Uri(WebApp.BaseAddress),
                               "api/ConceptoNomina/InsertarDiasLaboradosNomina");
                }

                var listaErrores = lista.Where(x => x.Valido == false).ToList();
                if (listaErrores.Count > 0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{Mensaje.DiasLaboradosConErrores}|{"12000"}";
                }
                else
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Success}|{Mensaje.Satisfactorio}";
                }

                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
            }

        }

        private async Task<FileInfo> SubirFichero(List<IFormFile> files,string folder)
        {
            byte[] data;
            using (var br = new BinaryReader(files[0].OpenReadStream()))
                data = br.ReadBytes((int)files[0].OpenReadStream().Length);
            string sFileName = files[0].FileName;
            await uploadFileService.UploadFile(data, folder, Convert.ToString(ObtenerCalculoNomina().Result.IdCalculoNomina), ".xlsx");
            string sWebRootFolder = _hostingEnvironment.WebRootPath;

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, string.Format("{0}/{1}.{2}", folder, ObtenerCalculoNomina().Result.IdCalculoNomina, "xlsx")));
            return file;
        }


        private async Task<List<DiasLaboradosNomina>> LeerExcelDiasLaborados(FileInfo file)
        {
            try
            {
                var lista = new List<DiasLaboradosNomina>();
                var listaSalida = new List<DiasLaboradosNomina>();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    var idCalculoNomina = ObtenerCalculoNomina().Result.IdCalculoNomina;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var identificacionEmpleado = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString();
                        var cantidadStr = worksheet.Cells[row, 2].Value == null ? Convert.ToString(0.0) : worksheet.Cells[row, 2].Value.ToString();

                        cantidadStr = cantidadStr.Replace(",", ",");
                        var cantidad = Convert.ToInt32(cantidadStr);


                        lista.Add(new DiasLaboradosNomina
                        {
                           IdCalculoNomina=idCalculoNomina,
                           CantidadDias=cantidad,
                           IdentificacionEmpleado=identificacionEmpleado,

                        });
                    }

                    listaSalida = await apiServicio.Listar<DiasLaboradosNomina>(lista, new Uri(WebApp.BaseAddress),
                                                   "api/ConceptoNomina/VerificarExcelDiasLaborados");

                }
                return listaSalida;

            }
            catch (Exception ex)
            {
                return new List<DiasLaboradosNomina>();
            }
        }

        private async Task<List<HorasExtrasNomina>> LeerExcelHorasExtras(FileInfo file)
        {
            try
            {
                var lista = new List<HorasExtrasNomina>();
                var listaSalida = new List<HorasExtrasNomina>();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    var idCalculoNomina = ObtenerCalculoNomina().Result.IdCalculoNomina;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var identificacionEmpleado = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString();
                        var cantidadStr = worksheet.Cells[row, 2].Value == null ? Convert.ToString(0.0) : worksheet.Cells[row, 2].Value.ToString();
                        var EsExtraordinaria = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString();


                        cantidadStr = cantidadStr.Replace(",", ",");
                        var cantidad = Convert.ToInt32(cantidadStr);
                        

                        lista.Add(new HorasExtrasNomina
                        {
                            IdCalculoNomina=idCalculoNomina,
                            CantidadHoras=cantidad,
                            IdentificacionEmpleado=identificacionEmpleado,
                            EsExtraordinaria= EsExtraordinaria=="0" ? false:true
                            
                        });
                    }

                    listaSalida = await apiServicio.Listar<HorasExtrasNomina>(lista, new Uri(WebApp.BaseAddress),
                                                   "api/ConceptoNomina/VerificarExcelHorasExtras");

                }
                return listaSalida;

            }
            catch (Exception ex)
            {
                return new List<HorasExtrasNomina>();
            }
        }

        private async Task<List<ReportadoNomina>> LeerExcel(FileInfo file)
        {
            try
            {
                var lista = new List<ReportadoNomina>();
                var listaSalida = new List<ReportadoNomina>();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    var idCalculoNomina = ObtenerCalculoNomina().Result.IdCalculoNomina;
                    for (int row = 2; row <= rowCount; row++)
                    {
                     var codigoConcepto =worksheet.Cells[row, 1].Value ==null ? "" : worksheet.Cells[row, 1].Value.ToString();
                     var identificacionEmpleado = worksheet.Cells[row, 2].Value==null ? "" : worksheet.Cells[row, 2].Value.ToString();
                     var nombreEmpleado =worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString();
                     var cantidadStr = worksheet.Cells[row, 4].Value == null ? Convert.ToString(0.0) : worksheet.Cells[row, 4].Value.ToString() ;
                     var importeStr =worksheet.Cells[row, 5].Value == null ? Convert.ToString(0.0) :worksheet.Cells[row, 5].Value.ToString();

                        cantidadStr = cantidadStr.Replace(",", ",");
                        importeStr = importeStr.Replace(",", ",");
                        var cantidad = Convert.ToDouble(cantidadStr);
                        var importe = Convert.ToDouble(importeStr);

                        lista.Add(new ReportadoNomina
                        {
                            CodigoConcepto = codigoConcepto,
                            IdentificacionEmpleado = identificacionEmpleado,
                            NombreEmpleado = nombreEmpleado,
                            Cantidad = cantidad,
                            Importe = importe,
                            IdCalculoNomina = idCalculoNomina,
                            Valido = true,
                        });


                        

                    }

                     listaSalida = await apiServicio.Listar<ReportadoNomina>(lista, new Uri(WebApp.BaseAddress),
                                                    "api/ConceptoNomina/VerificarExcel");
                    
                }
                return listaSalida;
                
            }
            catch (Exception ex)
            {
                return new List<ReportadoNomina>();
            }
        }

        public async Task<IActionResult> LimpiarDiasLaborados()
        {
            try
            {
                var response = await apiServicio.ObtenerElementoAsync1<Response>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress)
                                                                          , "api/CalculoNomina/LimpiarDiasLaborados");
                if (response.IsSuccess)
                {
                    return this.Redireccionar("CalculoNomina", "DiasLaborados", $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.Excepcion}");
            }
        }

        public async Task<IActionResult> LimpiarReportados()
        {
            try
            {
                    var response = await apiServicio.ObtenerElementoAsync1<Response>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress)
                                                                              , "api/CalculoNomina/LimpiarReportados");
                    if (response.IsSuccess)
                    {
                        return this.Redireccionar("CalculoNomina", "ReportadoNomina", $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.Excepcion}");
            }
        }

        public async Task<IActionResult> LimpiarHorasExtras()
        {
            try
            {
                var response = await apiServicio.ObtenerElementoAsync1<Response>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress)
                                                                          , "api/CalculoNomina/LimpiarHorasExtras");
                if (response.IsSuccess)
                {
                    return this.Redireccionar("CalculoNomina", "HorasExtras",$"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.Excepcion}");
            }
        }


        private async Task CargarEmpleadosActivos()
        {
            ViewData["Empleados"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>( new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosActivos"), "IdEmpleado", "NombreApellido");
        }

        public async Task<JsonResult> CargarConceptosActivos(int idEmpleado)
        {
            var emleado = new Empleado { IdEmpleado = Convert.ToInt32(idEmpleado) };
           var conceptos= await apiServicio.Listar<ConceptoNomina>(emleado, new Uri(WebApp.BaseAddress), "api/ConceptoNomina/ListarConceptoNominaPorTipoRelacionDelEmpleado");
            return Json(conceptos);
        }


        public async Task<IActionResult> AdicionarReportados()
        {
            try
            {

                await CargarEmpleadosActivos();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarReportados(AdicionarReportadoNominaViewModel adicionarReportadoNomina)
        {
            try
            {
                var a = await ObtenerCalculoNomina();
                adicionarReportadoNomina.IdCalculoNomina = a.IdCalculoNomina;
                var reportadoRequest = await apiServicio.InsertarAsync<Response>(adicionarReportadoNomina, new Uri(WebApp.BaseAddress),
                               "api/ConceptoNomina/InsertarReportadoNominaIndividual");

                if (!reportadoRequest.IsSuccess)
                {
                    await CargarEmpleadosActivos();
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.NoProcesarSolicitud}|{"45000"}";
                    return View();
                }
                await CargarEmpleadosActivos();
                this.TempData["MensajeTimer"] = $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}|{"45000"}";
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> AdicionarHorasExtras()
        {
            try
            {
               
                await CargarEmpleadosActivos();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarHorasExtras(HorasExtrasNomina horasExtrasNomina)
        {
            try
            {
                var a =await ObtenerCalculoNomina();
                horasExtrasNomina.IdCalculoNomina =a.IdCalculoNomina;
                var reportadoRequest = await apiServicio.InsertarAsync<Response>(horasExtrasNomina, new Uri(WebApp.BaseAddress),
                               "api/ConceptoNomina/InsertarHorasExtrasNominaPorEmpleado");

                if (!reportadoRequest.IsSuccess)
                {
                    await CargarEmpleadosActivos();
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.NoProcesarSolicitud}|{"45000"}";
                    return View();
                }
                await CargarEmpleadosActivos();
                this.TempData["MensajeTimer"] = $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}|{"45000"}";
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> DiasLaboradosBase()
        {
            try
            {
                var lista = await apiServicio.Listar<DiasLaboradosNomina>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress)
                                                                          , "api/CalculoNomina/ListarDiasLaborados");
                if (lista.Count == 0)
                {
                    return this.Redireccionar("CalculoNomina", "DiasLaborados", $"{Mensaje.Aviso}|{Mensaje.NoExistenRegistros}");
                }
                return View(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> HorasExtrasBase()
        {
            try
            {
                var lista = await apiServicio.Listar<HorasExtrasNomina>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress)
                                                                          , "api/CalculoNomina/ListarHorasExtras");
                if (lista.Count == 0)
                {
                    return this.Redireccionar("CalculoNomina", "HorasExtras", $"{Mensaje.Aviso}|{Mensaje.NoExistenRegistros}");
                }
                return View(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> MostrarExcelBase()
        {
            try
            {
                    var lista = await apiServicio.Listar<ReportadoNomina>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress)
                                                                              , "api/CalculoNomina/ListarReportados");
                    if (lista.Count == 0)
                    {
                        return this.Redireccionar("CalculoNomina","ReportadoNomina",$"{Mensaje.Aviso}|{Mensaje.NoExistenRegistros}");
                    }
                    return View(lista); 
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostrarExcel(CalculoNomina calculoNomina, List<IFormFile> files)
        {
            try
            {
                if (files.Count <= 0)
                {
                    return this.Redireccionar("CalculoNomina", "ReportadoNomina", new { id = Convert.ToString(ObtenerCalculoNomina().Result.IdCalculoNomina) }, $"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
                }


                var file = await SubirFichero(files, "DocumentoNomina/Reportados");
                var lista = await LeerExcel(file);
                if (lista.Count==0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ReportadoNoCumpleFormato}|{"45000"}";
                    return View(lista);
                }
                var listaSalvar = lista.Where(x => x.Valido == true).ToList();
                var reportadoRequest = new Response();
                if (listaSalvar.Count > 0)
                {
                    reportadoRequest = await apiServicio.InsertarAsync<Response>(listaSalvar, new Uri(WebApp.BaseAddress),
                               "api/ConceptoNomina/InsertarReportadoNomina");
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
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostrarHorasExtras(CalculoNomina calculoNomina, List<IFormFile> files)
        {
            try
            {
                if (files.Count <= 0)
                {
                    return this.Redireccionar("CalculoNomina", "HorasExtras", new { id = Convert.ToString(ObtenerCalculoNomina().Result.IdCalculoNomina) }, $"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
                }
                var file = await SubirFichero(files, "DocumentoNomina/HorasExtras");
                var lista = await LeerExcelHorasExtras(file);
                if (lista.Count == 0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.HorasExtrasNoCumpleFormato}|{"45000"}";
                    return View(lista);
                }
                var listaSalvar = lista.Where(x => x.Valido == true).ToList();
                var reportadoRequest = new Response();
                if (listaSalvar.Count > 0)
                {
                    reportadoRequest = await apiServicio.InsertarAsync<Response>(listaSalvar, new Uri(WebApp.BaseAddress),
                               "api/ConceptoNomina/InsertarHorasExtrasNomina");
                }

                var listaErrores = lista.Where(x => x.Valido == false).ToList();
                if (listaErrores.Count > 0)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{Mensaje.HorasExtrasConErrores}|{"12000"}";
                }
                else
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Success}|{Mensaje.Satisfactorio}";
                }

                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
            }

        }

        public async Task<IActionResult> Create(string mensaje)
        {
            await CargarComboxProcesoPeriodo();
            var vista = new CalculoNomina { DecimoCuartoSueldo ="NINGUNO", DecimoTercerSueldo = false,EmpleadoActivo=true};
            return View(vista);
        }

        public async Task CargarComboxProcesoPeriodo()
        {
            ViewData["Procesos"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina"), "IdProceso", "Descripcion");
            ViewData["Periodos"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<PeriodoNomina>(new Uri(WebApp.BaseAddress), "api/PeriodoNomina/ListarPeriodoNomina"), "IdPeriodo", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalculoNomina CalculoNomina)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(CalculoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/CalculoNomina/InsertarCalculoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                await CargarComboxProcesoPeriodo();
                return View(CalculoNomina);

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }



        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                    if (HttpContext.Session.GetInt32(Constantes.IdCalculoNominaSession) != id)
                    {
                        HttpContext.Session.SetInt32(Constantes.IdCalculoNominaSession, id);

                        var calculoNomina = new CalculoNomina { IdCalculoNomina = id };

                        var calculoRespuesta = await apiServicio.ObtenerElementoAsync1<Response>(calculoNomina, new Uri(WebApp.BaseAddress),
                                      "api/CalculoNomina/ObtenerCalculoNomina");

                        var calculoRespuestaD = JsonConvert.DeserializeObject<CalculoNomina>(calculoRespuesta.Resultado.ToString());

                        HttpContext.Session.SetString(Constantes.DescripcionCalculoNominaSession, calculoRespuestaD.Descripcion);

                    }
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(await ObtenerCalculoNomina(), new Uri(WebApp.BaseAddress),
                                                                  "api/CalculoNomina/ObtenerCalculoNomina");
                    if (respuesta.IsSuccess)
                    {
                         
                        var vista = JsonConvert.DeserializeObject<CalculoNomina>(respuesta.Resultado.ToString());
                        await CargarComboxProcesoPeriodo();
                        return View(vista);
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
                var calculoNomina = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(calculoNomina, new Uri(WebApp.BaseAddress)
                                                               , "api/CalculoNomina/EliminarCalculoNomina");
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



        public async Task<IActionResult> EliminarReportado(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }

                var reportadoNomina = new ReportadoNomina { IdReportadoNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(reportadoNomina, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoNomina/EliminarReportado");
                if (response.IsSuccess)
                {
                    return this.Redireccionar("CalculoNomina", "MostrarExcelBase", $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar("CalculoNomina", "MostrarExcelBase", $"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }


        public async Task<IActionResult> EliminarDiasLaborados(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }

                var horaExtra = new DiasLaboradosNomina { IdDiasLaboradosNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(horaExtra, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoNomina/EliminarDiasLaborados");
                if (response.IsSuccess)
                {
                    return this.Redireccionar("CalculoNomina", "DiasLaboradosBase", $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar("CalculoNomina", "DiasLaboradosBase", $"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        public async Task<IActionResult> EliminarHoraExtra(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }

                var horaExtra = new HorasExtrasNomina { IdHorasExtrasNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(horaExtra, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoNomina/EliminarHoraExtra");
                if (response.IsSuccess)
                {
                    return this.Redireccionar("CalculoNomina","HorasExtrasBase",$"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar("CalculoNomina", "HorasExtrasBase",$"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }



        public async Task<IActionResult> EditarDiasLaborados(int? id)
        {
            if (id==null)
            {
                return this.Redireccionar("CalculoNomina", "DiasLaboradosBase", $"{Mensaje.Aviso}|{"Debe seleccionar un registro"}");
            }
            var diasLaboradosDB = new DiasLaboradosNomina { IdDiasLaboradosNomina = Convert.ToInt32(id) };
            var response =await apiServicio.ObtenerElementoAsync1<Response>(diasLaboradosDB, new Uri(WebApp.BaseAddress),
                                                                 "api/CalculoNomina/ObtenerDiasLaborados");

            var diasLaborados = JsonConvert.DeserializeObject<DiasLaboradosNomina>(response.Resultado.ToString());

            return View(diasLaborados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDiasLaborados(DiasLaboradosNomina diasLaboradosNomina)
        {
            if (diasLaboradosNomina==null)
            {
                return this.Redireccionar("CalculoNomina", "DiasLaboradosBase", $"{Mensaje.Aviso}|{"No se ha podido editar..."}");
            }
           
            var response = await apiServicio.EditarAsync<Response>(diasLaboradosNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/CalculoNomina/EditarDiasLaborados");

            if (response.IsSuccess)
            {
                return this.Redireccionar("CalculoNomina", "DiasLaboradosBase", $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
            }
            return this.Redireccionar("CalculoNomina", "DiasLaboradosBase", $"{Mensaje.Error}|{Mensaje.ErrorEditar}");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CalculoNomina CalculoNomina)
        {
            Response response = new Response();
            try
            {
                    response = await apiServicio.EditarAsync<Response>(CalculoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/CalculoNomina/EditarCalculoNomina");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar("CalculoNomina","Detalle",new { id=ObtenerCalculoNomina().Result.IdCalculoNomina},$"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarComboxProcesoPeriodo();
                    return View(CalculoNomina);
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
                
                var lista = await apiServicio.Listar<CalculoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/CalculoNomina/ListarCalculoNomina");
                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> ConceptoConjunto(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var tipoConjuntoEliminar = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/CalculoNomina/EliminarCalculoNomina");
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

        public async Task<CalculoNomina> ObtenerCalculoNomina()
        {

            var calculoNomina = new CalculoNomina
            {
                IdCalculoNomina = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.IdCalculoNominaSession)),
                Descripcion = HttpContext.Session.GetString(Constantes.DescripcionCalculoNominaSession),

            };
            return calculoNomina;
        }

    }
}