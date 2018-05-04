using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class TipoGastoPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;

        public TipoGastoPersonalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public IActionResult Create(string mensaje)
        {
             
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoDeGastoPersonal TipoDeGastoPersonal)
        {
            if (!ModelState.IsValid)
            {
                return View(TipoDeGastoPersonal);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoDeGastoPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TipoDeGastoPersonal/InsertarTipoDeGastoPersonal");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                return View(TipoDeGastoPersonal);

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorListado}");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var TipoDeGastoPersonal = new TipoDeGastoPersonal { IdTipoGastoPersonal = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(TipoDeGastoPersonal, new Uri(WebApp.BaseAddress),
                                                                  "api/TipoDeGastoPersonal/ObtenerTipoDeGastoPersonal");
                    if (respuesta.IsSuccess)
                    {
                        var vista = JsonConvert.DeserializeObject<TipoDeGastoPersonal>(respuesta.Resultado.ToString());
                        return View(vista);
                    }
                }

                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TipoDeGastoPersonal TipoDeGastoPersonal)
        {

            if (!ModelState.IsValid)
            {
                return View(TipoDeGastoPersonal);
            }
            Response response = new Response();
            try
            {
                if (TipoDeGastoPersonal.IdTipoGastoPersonal > 0)
                {
                    response = await apiServicio.EditarAsync<Response>(TipoDeGastoPersonal, new Uri(WebApp.BaseAddress),
                                                                 "api/TipoDeGastoPersonal/EditarTipoDeGastoPersonal");

                    if (response.IsSuccess)
                    {
                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    return View(TipoDeGastoPersonal);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            try
            {
                 
                var lista = await apiServicio.Listar<TipoDeGastoPersonal>(new Uri(WebApp.BaseAddress)
                                                                     , "api/TipoDeGastoPersonal/ListarTipoDeGastoPersonal");
                return View(lista);
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
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
                }
                var tipoConjuntoEliminar = new TipoDeGastoPersonal { IdTipoGastoPersonal = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/TipoDeGastoPersonal/EliminarTipoDeGastoPersonal");
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
    }
}