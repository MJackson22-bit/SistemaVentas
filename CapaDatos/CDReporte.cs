using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDReporte
    {
        public List<ReporteCompra> GetCompras(string fechaInicio, string fechaFin, int idProveedor)
        {
            List<ReporteCompra> lista = new List<ReporteCompra>(); ;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    SqlCommand cmd = new SqlCommand("SP_REPORTECOMPRAS", connection);
                    cmd.Parameters.AddWithValue("Fecha_Inicio", fechaInicio);
                    cmd.Parameters.AddWithValue("Fecha_Fin", fechaFin);
                    cmd.Parameters.AddWithValue("IdProveedor", idProveedor);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteCompra()
                            {
                                FechaRegistro = dr["Fecha_Registro"].ToString(),
                                TipoDocumento = dr["Tipo_Documento"].ToString(),
                                NumeroDocumento = dr["Numero_Documento"].ToString(),
                                MontoTotal = dr["Monto_Total"].ToString(),
                                UsuarioRegistro = dr["Usuario_Registro"].ToString(),
                                DocumentoProveedor = dr["Documento_Proveedor"].ToString(),
                                RazonSocial = dr["Razon_Social"].ToString(),
                                CodigoProducto = dr["Codigo_Producto"].ToString(),
                                NombreProducto = dr["Nombre_Producto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                PrecioCompra = dr["Precio_Compra"].ToString(),
                                PrecioVenta = dr["Precio_Venta"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                SubTotal = dr["Sub_Total"].ToString()
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    lista = new List<ReporteCompra>();
                }
            }
            return lista;
        }
        public List<ReporteVenta> GetVentas(string fechaInicio, string fechaFin)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>(); ;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    SqlCommand cmd = new SqlCommand("SP_REPORTEVENTAS", connection);
                    cmd.Parameters.AddWithValue("Fecha_Inicio", fechaInicio);
                    cmd.Parameters.AddWithValue("Fecha_Fin", fechaFin);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVenta()
                            {
                                FechaRegistro = dr["Fecha_Registro"].ToString(),
                                TipoDocumento = dr["Tipo_Documento"].ToString(),
                                NumeroDocumento = dr["Numero_Documento"].ToString(),
                                MontoTotal = dr["Monto_Total"].ToString(),
                                UsuarioRegistro = dr["Usuario_Registro"].ToString(),
                                DocumentoCliente = dr["Documento_Cliente"].ToString(),
                                NombreCliente = dr["Nombre_Cliente"].ToString(),
                                CodigoProducto = dr["Codigo_Producto"].ToString(),
                                NombreProducto = dr["Nombre_Producto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                PrecioVenta = dr["Precio_Venta"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                SubTotal = dr["Sub_Total"].ToString()
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    lista = new List<ReporteVenta>();
                }
            }
            return lista;
        }
    }
}
