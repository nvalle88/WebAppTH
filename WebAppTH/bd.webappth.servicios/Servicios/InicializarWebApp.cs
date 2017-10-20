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

        private static async Task<Adscsist> ObtenerHostSistema(string id, Uri baseAddreess)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddreess;
                var url = string.Format("{0}/{1}", "/api/Adscsists", id);
                var respuesta = await client.GetAsync(url);

                var resultado = await respuesta.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response>(resultado);
                var sistema = JsonConvert.DeserializeObject<Adscsist>(response.Resultado.ToString());
                return sistema;
            }
        }

        public static async Task InicializarWeb(string id, Uri baseAddreess)
        {
            try
            {
                var sistema= await ObtenerHostSistema(id, baseAddreess);
                WebApp.BaseAddress = sistema.AdstHost;
                //WebApp.BaseAddress = "http://localhost:6000";
                //WebApp.BaseAddressRM = "http://localhost:9000";

            }
            catch (Exception ex)
            {

            }

        }

        public static async Task InicializarWebRecursosMateriales(string id, Uri baseAddreess)
        {
            try
            {
                var sistema = await ObtenerHostSistema(id, baseAddreess);
                WebApp.BaseAddressRM = sistema.AdstHost;
                //WebApp.BaseAddress = "http://localhost:6000";
                //WebApp.BaseAddressRM = "http://localhost:9000";

            }
            catch (Exception ex)
            {

            }

        }


        public static async Task InicializarLogEntry(string id, Uri baseAddress)
        {
            try
            {
                var sistema = await ObtenerHostSistema(id, baseAddress);
                AppGuardarLog.BaseAddress = sistema.AdstHost;

            }
            catch (Exception ex)
            {

            }

        }


        #endregion
    }
}