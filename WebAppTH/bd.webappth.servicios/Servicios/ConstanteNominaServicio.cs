using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Utils.Seguridad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Interfaces
{
    public class ConstanteNominaServicio : IConstantesNomina
    {

        public async Task<List<ConstanteNomina>> Listar(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var uri = string.Format("{0}/{1}", WebApp.BaseAddress, url);
                    var response = await client.GetAsync(new Uri(uri));
                    var resultado = await response.Content.ReadAsStringAsync();
                    var respuesta = JsonConvert.DeserializeObject<List<ConstanteNomina>>(resultado);
                    return respuesta;
                }

            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
