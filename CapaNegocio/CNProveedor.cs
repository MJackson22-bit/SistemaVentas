using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNProveedor
    {
        private CDProveedor objectProveedor = new CDProveedor();
        public List<Proveedor> listar()
        {
            return objectProveedor.listar();
        }
        public int Registar(Proveedor Proveedor, out string message)
        {
            message = string.Empty;
            if (Proveedor.Documento == "")
            {
                message += "Es necesario el documento de Proveedor\n";
            }
            if (Proveedor.RazonSocial == "")
            {
                message += "Es necesario la Razon Socila del Proveedor\n";
            }
            if (Proveedor.Correo == "")
            {
                message += "Es necesario el correo del Proveedor\n";
            }
            if (message != string.Empty)
            {
                return 0;
            }
            else
            {
                return objectProveedor.Registrar(Proveedor, out message);
            }
        }
        public bool Editar(Proveedor Proveedor, out string message)
        {
            message = string.Empty;
            if (Proveedor.Documento == "")
            {
                message += "Es necesario el documento de Proveedor\n";
            }
            if (Proveedor.RazonSocial == "")
            {
                message += "Es necesario la Razon Socila del Proveedor\n";
            }
            if (Proveedor.Correo == "")
            {
                message += "Es necesario el correo del Proveedor\n";
            }
            if (message != string.Empty)
            {
                return false;
            }
            else
            {
                return objectProveedor.Editar(Proveedor, out message);
            }
        }
        public bool Eliminar(Proveedor Proveedor, out string message)
        {
            message = string.Empty;
            return objectProveedor.Eliminar(Proveedor, out message);
        }
    }
}
