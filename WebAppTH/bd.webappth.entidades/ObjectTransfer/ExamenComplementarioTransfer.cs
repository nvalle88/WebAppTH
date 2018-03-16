using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ObjectTransfer
{
    public class ExamenComplementarioTransfer
    {
        /* Campos de la tabla*/

        public int IdExamenComplementario { get; set; }
        public DateTime Fecha { get; set; }
        public string Resultado { get; set; }
        public int IdTipoExamenComplementario { get; set; }
        public int IdFichaMedica { get; set; }
        public string Url { get; set; }


        /* Campos para el fichero*/

        public string Descripcion { get; set; }
        public string Extension { get; set; }
        public byte[] Fichero { get; set; }
    }
}
