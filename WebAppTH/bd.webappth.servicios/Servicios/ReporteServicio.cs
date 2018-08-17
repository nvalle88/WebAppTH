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
using bd.webappth.servicios.Extensores;

namespace bd.webappth.servicios.Servicios
{
    public class ReporteServicio : IReporteServicio
    {
        private IEncriptarServicio encriptarServicio;

        public ReporteServicio(IEncriptarServicio encriptarServicio)
        {
            this.encriptarServicio = encriptarServicio;
        }

        public Dictionary<string, string> AddParameters(string Key, string Value, Dictionary<string, string> parameters)
        {
            parameters.Add(encriptarServicio.Encriptar(Key), encriptarServicio.Encriptar(Value));
            return parameters;
        }

        public string GenerateUri(string ProjectReportUrl, Dictionary<string, string> parameters)
        {
            var newUri = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(ProjectReportUrl, parameters);
            return newUri;
        }

        public string GenerateUri(Dictionary<string, string> parameters)
        {
            var newUri = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(ReportConfig.ProjectReportUrl, parameters);
            return newUri;
        }

        public Dictionary<string, string> GetDefaultParameters(string FolderAndNameReport)
        {
            var parametersToAdd = new Dictionary<string, string> { { encriptarServicio.Encriptar("ServerReport"), encriptarServicio.Encriptar(ReportConfig.ReportServerUrl) }, { encriptarServicio.Encriptar("UsuarioReporte"), encriptarServicio.Encriptar(ReportConfig.UserName) }, { encriptarServicio.Encriptar("ContrasenaReporte"), encriptarServicio.Encriptar(ReportConfig.Password) }, { encriptarServicio.Encriptar("NombreReporte"), encriptarServicio.Encriptar(FolderAndNameReport) } };
            return parametersToAdd;
        }
    }
}
