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
            message = string.Empty;
            if(usuario.NombreCompleto == "")
            {
                message += "Es necesario el nombre de usuario\n";
            }
            if (usuario.Documento == "")
            {
                message += "Es necesario el documento de usuario\n";
            }
            if (usuario.Clave == "")
            {
                message += "Es necesario la clave del usuario\n";
            }
            if(message != string.Empty)
            {
                return 0;
            }
            else
            {
                return objectUsuario.Registrar(usuario, out message);
            }
        }
        public bool Editar(Usuario usuario, out string message)
        {
            message = string.Empty;
            if (message != string.Empty)
            {
                return false;
            }
            else
            {
                return objectUsuario.Editar(usuario, out message);
            }
        }
        public bool Eliminar(Usuario usuario, out string message)
        {
            message = string.Empty;
            return objectUsuario.Eliminar(usuario, out message);
        }
    }
}
