using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class EmpleadoViewModel
    {
        public Persona Persona { get; set; }
        public List<EmpleadoFamiliar> EmpleadoFamiliar { get; set; }
        //List<EmpleadoFamiliar> Empleadofamiliar { get; set; }
        public Empleado Empleado { get; set; }
        public DatosBancarios DatosBancarios { get; set; }
        public TrayectoriaLaboral TrayectoriaLaboral { get; set; }
        public IndiceOcupacionalModalidadPartida IndiceOcupacionalModalidadPartida { get; set; }



        //public void ejemplo(AccionPersonal, bd,char )
        //{

        //    Empleadofamiliar.Add(new 
        //        EmpleadoFamiliar
        //            {
        //               Persona= new Persona
        //               {
        //                   Identificacion
        //               }
        //            }

        //    )
        //}
    }

    
}
