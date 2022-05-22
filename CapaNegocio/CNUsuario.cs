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
        public int Registar(Usuario usuario, out string message)
        {
            return objectUsuario.Registrar(usuario, out message);
        }
        public bool Editar(Usuario usuario, out string message)
        {
            return objectUsuario.Editar(usuario, out message);
        }
        public bool Eliminar(Usuario usuario, out string message)
        {
            return objectUsuario.Eliminar(usuario, out message);
        }
    }
}
