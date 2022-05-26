using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNVenta
    {

        private CDVenta objectVenta = new CDVenta();
        public int getCorelative()
        {
            return objectVenta.getCorelative();
        }
        public bool Registar(Venta venta, DataTable detalleVenta, out string message)
        {
            return objectVenta.Regsitrar(venta, detalleVenta, out message);
        }
    }
}
