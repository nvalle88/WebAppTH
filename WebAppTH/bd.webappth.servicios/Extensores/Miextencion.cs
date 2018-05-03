using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.servicios.Extensores
{
   public static class Miextencion
    {
        public static int Cantidad(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
