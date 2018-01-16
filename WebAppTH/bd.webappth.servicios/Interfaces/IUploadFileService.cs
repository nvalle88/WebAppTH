using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ObjectTransfer;
using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Interfaces
{
   public interface IUploadFileService
    {
        Task<bool> UploadFile(byte[] file, string folder, string fileName, string extension);
        bool DeleteFile(string folder, string fileName, string extension);
        NoticiaTransfer GetFile(string folder, string fileName, string extension);
    }
}
