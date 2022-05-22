using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNRol
    {
        private CNRol objectRol = new CNRol();
        public List<Rol> listar()
        {
            return objectRol.listar();
        }
    }
}
