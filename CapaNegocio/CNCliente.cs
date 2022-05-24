using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNCliente
    {
        private CDCliente objectCliente = new CDCliente();
        public List<Cliente> listar()
        {
            return objectCliente.listar();
        }
        public int Registar(Cliente cliente, out string message)
        {
            message = string.Empty;
            if (cliente.Documento == "")
            {
                message += "Es necesario el documento de Cliente\n";
            }
            if (cliente.NombreCompleto == "")
            {
                message += "Es necesario el nombre de Cliente\n";
            }
            if (cliente.Telefono == "")
            {
                message += "Es necesario el teléfono del Cliente\n";
            }
            if (cliente.Correo== "")
            {
                message += "Es necesario el correo del Cliente\n";
            }
            if (message != string.Empty)
            {
                return 0;
            }
            else
            {
                return objectCliente.Registrar(cliente, out message);
            }
        }
        public bool Editar(Cliente cliente, out string message)
        {
            message = string.Empty;
            if (cliente.Documento == "")
            {
                message += "Es necesario el documento de Cliente\n";
            }
            if (cliente.NombreCompleto == "")
            {
                message += "Es necesario el nombre de Cliente\n";
            }
            if (cliente.Telefono == "")
            {
                message += "Es necesario el teléfono del Cliente\n";
            }
            if (cliente.Correo == "")
            {
                message += "Es necesario el correo del Cliente\n";
            }
            if (message != string.Empty)
            {
                return false;
            }
            else
            {
                return objectCliente.Editar(cliente, out message);
            }
        }
        public bool Eliminar(Cliente Cliente, out string message)
        {
            message = string.Empty;
            return objectCliente.Eliminar(Cliente, out message);
        }
    }
}
