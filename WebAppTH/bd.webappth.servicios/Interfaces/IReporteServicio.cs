using bd.log.guardar.ObjectTranfer;
using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Interfaces
{
    public interface IReporteServicio
    {
        Dictionary<string, string> GetDefaultParameters(string FolderAndNameReport);
        Dictionary<string, string> AddParameters(string Key, string Value, Dictionary<string, string> parameters);
        string GenerateUri(string ProjectReportUrl, Dictionary<string, string> parameters);
        string GenerateUri(Dictionary<string, string> parameters);
    }
}
