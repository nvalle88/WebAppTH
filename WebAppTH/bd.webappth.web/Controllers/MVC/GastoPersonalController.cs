using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class GastoPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;


        public GastoPersonalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadosActivos");

                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> EditGastoPersonal(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var gastoPersonal = new GastoPersonal { IdGastoPersonal = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(gastoPersonal, new Uri(WebApp.BaseAddress),
                                                                  "api/GastoPersonal/ObtenerGastoPersonal");
                    if (respuesta.IsSuccess)
                    {
                        var vista = JsonConvert.DeserializeObject<GastoPersonal>(respuesta.Resultado.ToString());
                        await CargarCombox();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGastoPersonal(GastoPersonal gastoPersonal)
        {

            Response response = new Response();
            try
            {
                if (ObtenerGastoPersonal().IdEmpleado > 0)
                {
                    gastoPersonal.IdEmpleado = ObtenerGastoPersonal().IdEmpleado;
                    response = await apiServicio.EditarAsync<Response>(gastoPersonal, new Uri(WebApp.BaseAddress),
                                                                 "api/GastoPersonal/EditarGastoPersonal");

                    if (response.IsSuccess)
                    {
                        return this.Redireccionar("GastoPersonal", "GastoPersonalIndex", new { id = ObtenerGastoPersonal().IdEmpleado }, $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarCombox();
                    return View(gastoPersonal);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> DeleteConceptoConjunto(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var gastoPersonal = new GastoPersonal { IdGastoPersonal = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(gastoPersonal, new Uri(WebApp.BaseAddress)
                                                               , "api/GastoPersonal/EliminarGastoPersonal");
                if (response.IsSuccess)
                {
                    return this.Redireccionar("GastoPersonal", "GastoPersonalIndex", new { id = ObtenerGastoPersonal().IdEmpleado }, $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        public async Task<IActionResult> Historicos()
        {
            try
            {
                await CargarComboxHistoricos();
                return View();
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }


        public async Task<JsonResult> BuscarHistorico(string ano)
        {
            try
            {
                var gasto = await apiServicio.Listar<GastoPersonal>(new GastoPersonal { Ano =Convert.ToInt32(ano), IdEmpleado = ObtenerGastoPersonal().IdEmpleado }, new Uri(WebApp.BaseAddress)
                                                                    , "api/GastoPersonal/BuscarHistorico");

                return Json(gasto);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> GastoPersonalIndex(int id, string nombreEmpleado, string identificacion)
        {
            try
            {
                if (HttpContext.Session.GetInt32(Constantes.IdEmpleadoGastoPersonal) != id)
                {
                    HttpContext.Session.SetInt32(Constantes.IdEmpleadoGastoPersonal, id);
                    HttpContext.Session.SetString(Constantes.NombreEmpleadoGastoPersonal, nombreEmpleado);
                    HttpContext.Session.SetString(Constantes.IdentificacionGastoPersonal, identificacion);

                }
                var lista = await apiServicio.Listar<GastoPersonal>(new GastoPersonal { Ano = DateTime.Now.Year, IdEmpleado = ObtenerGastoPersonal().IdEmpleado }, new Uri(WebApp.BaseAddress)
                                                                    , "api/GastoPersonal/ListarGastoPersonal");

                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGastoPersonal(GastoPersonal gastoPersonal)
        {
            Response response = new Response();
            try
            {

                gastoPersonal.IdEmpleado = ObtenerGastoPersonal().IdEmpleado;
                gastoPersonal.Ano = DateTime.Now.Year;
                response = await apiServicio.EditarAsync<Response>(gastoPersonal, new Uri(WebApp.BaseAddress),
                                                             "api/GastoPersonal/InsertarGastoPersonal");

                if (response.IsSuccess)
                {
                    return this.Redireccionar("GastoPersonal", "GastoPersonalIndex", new { id = ObtenerGastoPersonal().IdEmpleado }, $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                await CargarCombox();
                return View(gastoPersonal);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }

        }

        public async Task<IActionResult> CreateGastoPersonal()
        {

            await CargarCombox();
            return View();
        }

        private async Task CargarCombox()
        {
            ViewData["IdTipoGastoPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoDeGastoPersonal>(new Uri(WebApp.BaseAddress), "api/TipoDeGastoPersonal/ListarTipoDeGastoPersonal"), "IdTipoGastoPersonal", "Descripcion");
        }

        private async Task CargarComboxHistoricos()
        {
            ViewData["IdHistoricos"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GastoPersonal>(new GastoPersonal { IdEmpleado = ObtenerGastoPersonal().IdEmpleado }, new Uri(WebApp.BaseAddress), "api/GastoPersonal/ListarAnosGastoPersonal"), "Ano", "Ano");
        }


        public GastoPersonal ObtenerGastoPersonal()
        {
            var gastoPersonal = new GastoPersonal
            {
                IdEmpleado = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.IdEmpleadoGastoPersonal)),
            };
            return gastoPersonal;
        }
    }
}