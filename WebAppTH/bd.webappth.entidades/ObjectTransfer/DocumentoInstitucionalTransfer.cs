using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ObjectTransfer
{
    public class DocumentoInstitucionalTransfer
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Extension { get; set; }
        public string Url { get; set; }
        public byte[] Fichero { get; set; }
    }
}
