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
    public class CDVenta
    {
        public int getCorelative()
        {
            int idCorelative = 0;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) + 1 from VENTA");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    idCorelative = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception e)
                {
                    idCorelative = 0;
                }
            }
            return idCorelative;
        }
        public bool RestartStock(int idproducto, int cantidad)
        {
            bool respuesta = true;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock - @cantidad where IdProducto = @idproducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("cantidad", cantidad);
                    cmd.Parameters.AddWithValue("idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public bool SumarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock + @cantidad where IdProducto = @idproducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("cantidad", cantidad);
                    cmd.Parameters.AddWithValue("idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public bool Regsitrar(Venta venta, DataTable detalleVenta, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARVENTA", connection);
                    cmd.Parameters.AddWithValue("IdUsuario", venta.OUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("Tipo_Documento", venta.TipoDocumento);
                    cmd.Parameters.AddWithValue("Numero_Documento", venta.NumeroDocumento);
                    cmd.Parameters.AddWithValue("Documento_Cliente", venta.DocumentoCliente);
                    cmd.Parameters.AddWithValue("Nombre_Cliente", venta.NombreCliente);
                    cmd.Parameters.AddWithValue("Monto_Pago", venta.MontoPago);
                    cmd.Parameters.AddWithValue("Monto_Cambio", venta.MontoCambio);
                    cmd.Parameters.AddWithValue("Monto_Total", venta.MontoTotal);
                    cmd.Parameters.AddWithValue("Detalle_Venta", detalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    message = cmd.Parameters["Mensaje"].Value.ToString();

                }
                catch (Exception e)
                {
                    respuesta = false;
                    message = e.Message;
                }
            }
            return respuesta;
        }
        public Venta GetVenta(string numero)
        {
            Venta venta = new Venta();
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select v.IdVenta, u.Nombre_Completo, v.Documento_Cliente, v.Nombre_Cliente,");
                    query.AppendLine("v.Tipo_Documento, v.Numero_Documento, v.Monto_Pago, v.Monto_Cambio, v.Monto_Total,");
                    query.AppendLine("convert(char(10), v.Fecha_Registro, 103)[Fecha_Registro] from VENTA v");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = v.IdUsuario where v.Numero_Documento = @numero");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("numero", numero);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            venta = new Venta()
                            {
                                IdVenta = int.Parse(dr["IdVenta"].ToString()),
                                OUsuario = new Usuario() { NombreCompleto = dr["Nombre_Completo"].ToString() },
                                DocumentoCliente = dr["Documento_Cliente"].ToString(),
                                NombreCliente = dr["Nombre_Cliente"].ToString(),
                                TipoDocumento = dr["Tipo_Documento"].ToString(),
                                NumeroDocumento = dr["Numero_Documento"].ToString(),
                                MontoPago = Convert.ToDecimal(dr["Monto_Pago"].ToString()),
                                MontoCambio= Convert.ToDecimal(dr["Monto_Cambio"].ToString()),
                                MontoTotal = Convert.ToDecimal(dr["Monto_Total"].ToString()),
                                FechaRegistro = dr["Fecha_Registro"].ToString()
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    venta = new Venta();
                }
            }
            return venta;
        }
        public List<DetalleVenta> GetDetalleVentas(int idVenta)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select p.Nombre, dv.Precio_Venta, dv.Cantidad, dv.Sub_Total from DETALLE_VENTA dv");
                    query.AppendLine("inner join PRODUCTO p on p.IdProducto = dv.IdProducto where dv.IdVenta = @idVenta");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("idVenta", idVenta);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVenta()
                            {
                                OProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                PrecioVenta = Convert.ToDecimal(dr["Precio_Venta"]),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                SubTotal = Convert.ToDecimal(dr["Sub_Total"])
                            });
                        };
                    }
                }
            }
            catch (Exception e)
            {
                lista = new List<DetalleVenta>();
            }
            return lista;
        }
    }
}
