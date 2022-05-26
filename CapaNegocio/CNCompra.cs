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
    public class CNCompra
    {
        private CDCompra objectCompra = new CDCompra();
        public int getCorelative()
        {
            return objectCompra.getCorelative();
        }
        public bool Registar(Compra compra, DataTable detalleCompra, out string message)
        {
            return objectCompra.Regsitrar(compra, detalleCompra, out message);
        }
        public Compra GetCompra(string numero)
        {
            Compra compra = objectCompra.GetCompra(numero);
            if(compra.IdCompra != 0)
            {
                List<DetalleCompra> lista = objectCompra.GetDetalleCompras(compra.IdCompra);
                compra.listaDetallesCompras = lista;
            }
            return compra;
        }
    }
}
