using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNCategoria
    {
        private CDCategoria objectCategoria = new CDCategoria();
        public List<Categoria> listar()
        {
            return objectCategoria.listar();
        }
        public int Registar(Categoria categoria, out string message)
        {
            message = string.Empty;
            if (categoria.Descripcion == "")
            {
                message += "Es necesario el nombre de Categoria\n";
            }
            if (message != string.Empty)
            {
                return 0;
            }
            else
            {
                return objectCategoria.Registrar(categoria, out message);
            }
        }
        public bool Editar(Categoria categoria, out string message)
        {
            message = string.Empty;
            if (categoria.Descripcion == "")
            {
                message += "Es necesario el nombre de Categoria\n";
            }
            if (message != string.Empty)
            {
                return false;
            }
            else
            {
                return objectCategoria.Editar(categoria, out message);
            }
        }
        public bool Eliminar(Categoria categoria, out string message)
        {
            message = string.Empty;
            return objectCategoria.Eliminar(categoria, out message);

        }
    }
}
