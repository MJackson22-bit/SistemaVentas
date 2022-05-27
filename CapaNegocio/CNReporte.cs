using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNReporte
    {
        private CDReporte reporte = new CDReporte();
        public List<ReporteCompra> GetCompras(string fechaInicio, string fechaFin, int idProveedor)
        {
            return reporte.GetCompras(fechaInicio, fechaFin, idProveedor);
        }
        public List<ReporteVenta> GetVentas(string fechaInicio, string fechaFin)
        {
            return reporte.GetVentas(fechaInicio, fechaFin);
        }
    }
}
