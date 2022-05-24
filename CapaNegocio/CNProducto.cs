using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNProducto
    {
        private CDProducto objectProducto = new CDProducto();
        public List<Producto> listar()
        {
            Console.WriteLine(objectProducto.ToString());
            return objectProducto.listar();
        }
        public int Registar(Producto producto, out string message)
        {
            message = string.Empty;
            if (producto.Codigo == "")
            {
                message += "Es necesario el Codigo del Producto\n";
            }
            if (producto.Nombre == "")
            {
                message += "Es necesario el Nombre del Producto\n";
            }
            if (producto.Descripcion == "")
            {
                message += "Es necesario la descripcion del Producto\n";
            }
            if (message != string.Empty)
            {
                return 0;
            }
            else
            {
                return objectProducto.Registrar(producto, out message);
            }
        }
        public bool Editar(Producto producto, out string message)
        {
            message = string.Empty;
            if (producto.Codigo == "")
            {
                message += "Es necesario el Codigo del Producto\n";
            }
            if (producto.Nombre == "")
            {
                message += "Es necesario el Nombre del Producto\n";
            }
            if (producto.Descripcion == "")
            {
                message += "Es necesario la descripcion del Producto\n";
            }
            if (message != string.Empty)
            {
                return false;
            }
            else
            {
                return objectProducto.Editar(producto, out message);
            }
        }
        public bool Eliminar(Producto producto, out string message)
        {
            message = string.Empty;
            return objectProducto.Eliminar(producto, out message);
        }
    }
}
