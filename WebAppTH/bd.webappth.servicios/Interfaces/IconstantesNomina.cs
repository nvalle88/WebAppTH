using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bd.webappth.servicios.Interfaces
{
   public interface IConstantesNomina
    {
        Task<List<ConstanteNomina>> Listar(string url);
    }
}
