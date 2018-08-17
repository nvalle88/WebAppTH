using bd.log.guardar.ObjectTranfer;
using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Interfaces
{
    public interface IEncriptarServicio
    {
        string Encriptar(string CadenaEncriptar);
        string Desencriptar(string cadenaDesencriptar);
    }
}
