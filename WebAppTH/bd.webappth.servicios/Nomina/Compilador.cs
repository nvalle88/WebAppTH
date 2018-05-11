using System;
using System.Data;
using System.Collections;
using bd.webappth.servicios.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using NCalc;

namespace bd.webappth.servicios.Nomina
{
    public class Compilador
    {
        private readonly IApiServicio apiServicio;
        private readonly IConstantesNomina constantesNomina;

        public Compilador(IApiServicio apiServicio, IConstantesNomina constantesNomina)
        {
            this.apiServicio = apiServicio;
            this.constantesNomina = constantesNomina;

        }

        public async Task<decimal> Evaluar(string expresion)
        {
            //DataTable dt = new DataTable();
          
           
            ///Busca y reemplaza constantes
            expresion = await CalculaConstantes(expresion);
            Expression e = new Expression(expresion);
            var d= e.Evaluate();
            //var v = dt.Compute(expresion, "");
            return Convert.ToDecimal(d);
        }

        private async Task<string> CalculaConstantes(string expresion)
        {
            string scape = "#";
            string result = expresion;
            string[] partes = expresion.Split(Convert.ToChar(scape));
            ArrayList constantes = new ArrayList();

            for (int i = 1; i < partes.Length; i++)
            {
                if (partes[i] != "")
                {
                    
                    string parte = partes[i].Replace("+", "").Replace("-", "").Replace("*", "").Replace("/", "").Replace("%", "").Replace(")", "").Replace("(", "");
                    string constante = scape + parte;
                    constantes.Add(constante.TrimEnd());

                }
            }

            var listaConstantes = await constantesNomina.Listar("api/ConstanteNomina/ListarConstanteNomina");
            double? valorConstante = null;
            foreach (string item in constantes)
            {

                if (item!="#")
                {

                    var elemeto = listaConstantes.Where(x => x.Constante == item).FirstOrDefault();

                    if (elemeto != null)
                    {
                        valorConstante = elemeto.Valor;
                    }
                    else
                    {
                        return null;
                    };
                    
                    result = result.Replace(item, Convert.ToString(valorConstante)); 
                }
            }
            return result;
        }

    }
}
