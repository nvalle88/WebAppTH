using bd.webappth.entidades.ObjectTransfer;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Servicios
{
    public class UploadFileService : IUploadFileService
    {
        private IHostingEnvironment _hostingEnvironment;


        public UploadFileService(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

       

        public async Task<bool> UploadFile(byte[] file, string folder, string fileName, string extension)
        {

            try
            {
                var stream = new MemoryStream(file);
                var a = string.Format("{0}/{1}.{2}", folder, fileName, extension);
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath,a );

                using (var fileStream = new FileStream(targetDirectory, FileMode.Create, FileAccess.Write))
                {
                   await stream.CopyToAsync(fileStream);
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public bool DeleteFile(string folder, string fileName, string extension)
        {
            try
            {
                var a = string.Format("{0}/{1}.{2}", folder, fileName, extension);
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, a);
                if (System.IO.File.Exists(targetDirectory))
                    {
                        System.IO.File.Delete(targetDirectory);
                        return true;
                    }
                
            }
            catch (Exception e)
            { }
            return false;
        }

        public NoticiaTransfer GetFile(string folder, string fileName, string extension)
        {
            var a = string.Format("{0}/{1}.{2}", folder, fileName, extension);
            var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, a);

            var file = new FileStream(targetDirectory, FileMode.Open);

            byte[] data;
            using (var br = new BinaryReader(file))
                data = br.ReadBytes((int)file.Length);

            var documenttransfer = new NoticiaTransfer
            {
                Titulo = "",
                Fichero = data,
            };
            return documenttransfer;


        }
    }
}
