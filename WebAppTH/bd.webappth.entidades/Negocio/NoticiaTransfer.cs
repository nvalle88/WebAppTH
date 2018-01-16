using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
    public class NoticiaTransfer
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        public byte[] Fichero { get; set; }
    }
}
