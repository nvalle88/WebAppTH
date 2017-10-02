using bd.log.guardar.Inicializar;
using bd.webappth.entidades.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Servicios
{
    public class InicializarWebApp
    {
        #region Methods

        public static async Task InicializarWeb(string id, Uri baseAddreess)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = baseAddreess;
                    var url = string.Format("{0}/{1}", "/api/Adscsists", id);
                    var respuesta = await client.GetAsync(url);

                    var resultado = await respuesta.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Response>(resultado);
                    var sistema = JsonConvert.DeserializeObject<Adscsist>(response.Resultado.ToString());
                    WebApp.BaseAddress = sistema.AdstHost;
                }
                //WebApp.BaseAddress = "http://localhost:55998";


            }
            catch (Exception ex)
            {

            }

        }


        public static async Task InicializarLogEntry(string id, Uri baseAddress)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = baseAddress;
                    var url = string.Format("{0}/{1}", "/api/Adscsists", id);
                    var respuesta = await client.GetAsync(url);

                    var resultado = await respuesta.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Response>(resultado);
                    var sistema = JsonConvert.DeserializeObject<Adscsist>(response.Resultado.ToString());
                    AppGuardarLog.BaseAddress = sistema.AdstHost;
                }
            }
            catch (Exception ex)
            {

            }

        }


        #endregion
    }
}
