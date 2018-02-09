using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using bd.webappth.entidades.Utils.Seguridad;

namespace bd.webappth.servicios.Servicios
{
    public class ApiServicio : IApiServicio
    {

        private async Task<bool> SalvarLog(LogEntryTranfer logEntryTranfer)
        {
            var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);
            if (responseLog.IsSuccess)
            {
                return true;
            }

            return false;
        }

        public async Task<Response> SalvarLog<T>(HttpContext context, EntradaLog model)
        {
            var NombreUsuario = "";
            try
            {
                var claim = context.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var menuRespuesta = await ObtenerElementoAsync1<log.guardar.Utiles.Response>(new ModuloAplicacion { Path = context.Request.Path, NombreAplicacion = WebApp.NombreAplicacion }, new Uri(WebApp.BaseAddress), "api/Adscmenus/GetMenuPadre");
                var menu = JsonConvert.DeserializeObject<Adscmenu>(menuRespuesta.Resultado.ToString());

                var Log = new LogEntryTranfer
                {
                    ApplicationName = WebApp.NombreAplicacion,
                    EntityID = menu.AdmeAplicacion,
                    ExceptionTrace = model.ExceptionTrace,
                    LogCategoryParametre = model.LogCategoryParametre,
                    LogLevelShortName = model.LogLevelShortName,
                    Message = context.Request.Path,
                    ObjectNext = model.ObjectNext,
                    ObjectPrevious = model.ObjectPrevious,
                    UserName = NombreUsuario,
                };
                var responseLog = await GuardarLogService.SaveLogEntry(Log);
                return new Response { IsSuccess = responseLog.IsSuccess };
            }
            catch (Exception ex)
            {
                var Log = new LogEntryTranfer
                {
                    ApplicationName = WebApp.NombreAplicacion,
                    EntityID = Mensaje.NoExisteModulo,
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = model.LogCategoryParametre,
                    LogLevelShortName = model.LogLevelShortName,
                    Message = context.Request.Path,
                    ObjectNext = model.ObjectNext,
                    ObjectPrevious = model.ObjectPrevious,
                    UserName = NombreUsuario,
                };
                var resultado = await SalvarLog(Log);
                return new Response { IsSuccess = resultado };
            }

        }

        public async Task<Response> InsertarAsync<T>(T model, Uri baseAddress, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> InsertarAsync<T>(object model, Uri baseAddress, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> EditarAsync<T>(object model, Uri baseAddress, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
            }
        }

        public async Task<T> ObtenerElementoAsync1<T>(object model, Uri baseAddress, string url) where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<T>(resultado);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response> ObtenerElementoAsync<T>(T model, Uri baseAddress, string url) where T :class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> EliminarAsync(object model, Uri baseAddress, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> EliminarAsync(string id, Uri baseAddress, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                   
                    url = string.Format("{0}/{1}", url, id);
                    var uri = string.Format("{0}/{1}", baseAddress, url);
                    var response = await client.DeleteAsync(new Uri(uri));
                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> EditarAsync<T>(string id,T model, Uri baseAddress, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                   
                    url = string.Format("{0}/{1}", url, id);
                    var uri = string.Format("{0}/{1}", baseAddress, url);
                    var response = await client.PutAsync(new Uri(uri), content);
                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<Response>(resultado);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
            }
        }

        public async Task<List<T>> Listar<T>(object model, Uri baseAddress, string url) where T : class
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = JsonConvert.SerializeObject(model);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");

                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var response = await client.PostAsync(new Uri(uri), content);

                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<List<T>>(resultado);
                    return respuesta;

                }

            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<T>> Listar<T>(Uri baseAddress, string url) where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var uri = string.Format("{0}/{1}", baseAddress, url);
                    var respuesta = await client.GetAsync(new Uri(uri));
                    var resultado = await respuesta.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<List<T>>(resultado);
                    return response ?? new List<T>();
                }
            }

                catch (Exception )
            {
                return new List<T>();
            }
                           
        }

        public async Task<T> SeleccionarAsync<T>(string id,Uri baseAddress, string url) where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    
                    url = string.Format("{0}/{1}", url, id);
                    var uri = string.Format("{0}/{1}", baseAddress, url);

                    var respuesta = await client.GetAsync(new Uri(uri));

                    var resultado = await respuesta.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<T>(resultado);
                    return response;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
