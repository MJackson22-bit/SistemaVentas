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
        public bool SumarStock(int idproducto, int cantidad)
        {
            return objectVenta.SumarStock(idproducto, cantidad);
        }
        public bool RestartStock(int idproducto, int cantidad)
        {
            return objectVenta.RestartStock(idproducto, cantidad);
        }
        public int getCorelative()
        {
            return objectVenta.getCorelative();
        }
        public bool Registar(Venta venta, DataTable detalleVenta, out string message)
        {
            return objectVenta.Regsitrar(venta, detalleVenta, out message);
        }
        public Venta GetVenta(string numero)
        {
            Venta venta = objectVenta.GetVenta(numero);
            if (venta.IdVenta!= 0)
            {
                List<DetalleVenta> lista = objectVenta.GetDetalleVentas(venta.IdVenta);
                venta.ListDetalleVenta= lista;
            }
            return venta;
        }
    }
}
