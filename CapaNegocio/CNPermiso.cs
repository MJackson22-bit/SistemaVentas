using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNPermiso
    {
        private CDPermiso objectPermiso = new CDPermiso();
        public List<Permiso> listar(int idUsuario)
        {
            return objectPermiso.listar(idUsuario);
        }
    }
}
