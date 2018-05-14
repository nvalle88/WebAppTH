using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
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


        public async Task<IActionResult> ReportadoNomina(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (HttpContext.Session.GetInt32(Constantes.IdCalculoNominaSession) !=Convert.ToInt32(id))
                    {
                        HttpContext.Session.SetInt32(Constantes.IdCalculoNominaSession, Convert.ToInt32(id));
                    }
                    var CalculoNomina = new CalculoNomina { IdCalculoNomina = ObtenerCalculoNomina().IdCalculoNomina };
                    return View(CalculoNomina);
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
            await uploadFileService.UploadFile(data, "DocumentoNomina/Reportados", Convert.ToString(ObtenerCalculoNomina().IdCalculoNomina), ".xlsx");
            string sWebRootFolder = _hostingEnvironment.WebRootPath;

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, string.Format("{0}/{1}.{2}", "DocumentoNomina/Reportados", ObtenerCalculoNomina().IdCalculoNomina, "xlsx")));
            return file;
        }


        private async Task<List<ReportadoNomina>> LeerExcel(FileInfo file)
        {
            try
            {
                var lista = new List<ReportadoNomina>();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
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
                      
                            var concepto = new ConceptoNomina { Codigo = codigoConcepto };
                            var conceptoRequest = await apiServicio.ObtenerElementoAsync1<Response>(concepto, new Uri(WebApp.BaseAddress),
                                                            "api/ConceptoNomina/ExisteConceptoPorCodigo");

                            var persona = new Persona { Identificacion = identificacionEmpleado };
                            var empleadoRequest = await apiServicio.ObtenerElementoAsync1<Response>(persona, new Uri(WebApp.BaseAddress),
                                                            "api/Empleados/ExisteEmpleadoPorIdentificacion");

                        var validar =0;
                            if (conceptoRequest.IsSuccess==false && empleadoRequest.IsSuccess==false)
                            {
                                lista.Add(new ReportadoNomina
                                {
                                    CodigoConcepto = codigoConcepto,
                                    IdentificacionEmpleado = identificacionEmpleado,
                                    NombreEmpleado = nombreEmpleado,
                                    Cantidad = cantidad,
                                    Importe = importe,
                                    IdCalculoNomina = ObtenerCalculoNomina().IdCalculoNomina,
                                    Valido = false,
                                    MensajeError=Mensaje.ConceptoNoExiste

                                });
                            validar = -1;
                        }
                        if (conceptoRequest.IsSuccess == true && empleadoRequest.IsSuccess == false)
                        {
                            lista.Add(new ReportadoNomina
                            {
                                CodigoConcepto = codigoConcepto,
                                IdentificacionEmpleado = identificacionEmpleado,
                                NombreEmpleado = nombreEmpleado,
                                Cantidad = cantidad,
                                Importe = importe,
                                IdCalculoNomina = ObtenerCalculoNomina().IdCalculoNomina,
                                Valido = false,
                                MensajeError = Mensaje.EmpleadoNoExiste

                            });
                            validar = -1;

                        }
                        if (conceptoRequest.IsSuccess == false && empleadoRequest.IsSuccess == true)
                        {
                            lista.Add(new ReportadoNomina
                            {
                                CodigoConcepto = codigoConcepto,
                                IdentificacionEmpleado = identificacionEmpleado,
                                NombreEmpleado = nombreEmpleado,
                                Cantidad = cantidad,
                                Importe = importe,
                                IdCalculoNomina = ObtenerCalculoNomina().IdCalculoNomina,
                                Valido = false,
                                MensajeError = Mensaje.ConceptoEmpleadoNoExiste

                            });
                            validar = -1;
                        }

                        if (validar==0)
                        {
                            lista.Add(new ReportadoNomina
                            {
                                CodigoConcepto = codigoConcepto,
                                IdentificacionEmpleado = identificacionEmpleado,
                                NombreEmpleado = nombreEmpleado,
                                Cantidad = cantidad,
                                Importe = importe,
                                IdCalculoNomina = ObtenerCalculoNomina().IdCalculoNomina,
                                Valido = true,
                            });  
                        }
                        
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                return new List<ReportadoNomina>();            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostrarExcel(CalculoNomina calculoNomina, List<IFormFile> files)
        {
            if (files.Count<=0)
            {
                return this.Redireccionar("CalculoNomina","ReportadoNomina",new  {id=Convert.ToString(ObtenerCalculoNomina().IdCalculoNomina) }, $"{Mensaje.Error}|{Mensaje.SeleccionarFichero}");
            }

           
            var file= await SubirFichero(files);
            var lista =await LeerExcel(file);
            var listaSalvar = lista.Where(x => x.Valido == true).ToList();
            var reportadoRequest = new Response();
            if (listaSalvar.Count>0)
            {
                 reportadoRequest = await apiServicio.InsertarAsync<Response>(listaSalvar, new Uri(WebApp.BaseAddress),
                            "api/ConceptoNomina/InsertarReportadoNomina"); 
            }
            var listaErrores = lista.Where(x => x.Valido == false).ToList();
            if (listaErrores.Count>0)
            {
                this.TempData["Mensaje"] = $"{Mensaje.Aviso}|{Mensaje.ReportadoConErrores}";
            }
            else
            {
                this.TempData["Mensaje"] = $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}";
            }

            return View(lista);

        }


        public async Task<IActionResult> Create(string mensaje)
        {
            await CargarComboxProcesoPeriodo();
            var vista = new CalculoNomina { Automatico = false, Reportado = false,EmpleadoActivo=true,EmpleadoPasivo=false };
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
            if (!ModelState.IsValid)
            {
                 
                return View(CalculoNomina);
            }
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



        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var CalculoNomina = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(CalculoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/CalculoNomina/ObtenerCalculoNomina");
                    if (respuesta.IsSuccess)
                    {
                         
                        var vista = JsonConvert.DeserializeObject<CalculoNomina>(respuesta.Resultado.ToString());
                        await CargarComboxProcesoPeriodo();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CalculoNomina CalculoNomina)
        {

            if (!ModelState.IsValid)
            {
                 
                return View(CalculoNomina);
            }
            Response response = new Response();
            try
            {
                
                    response = await apiServicio.EditarAsync<Response>(CalculoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/CalculoNomina/EditarCalculoNomina");

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

        public CalculoNomina ObtenerCalculoNomina()
        {
            var gastoPersonal = new CalculoNomina
            {
                IdCalculoNomina = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.IdCalculoNominaSession)),
            };
            return gastoPersonal;
        }

    }
}