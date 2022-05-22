using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNUsuario
    {
        private CDUsuario objectUsuario = new CDUsuario();
        public List<Usuario> listar()
        {
            return objectUsuario.listar();
        }
    }
}
