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
    public class CDCompra
    {
        public int getCorelative()
        {
            int idCorelative = 0;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) + 1 from COMPRA");
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
        public bool Regsitrar(Compra compra, DataTable detalleCompra, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCOMPRA", connection);
                    cmd.Parameters.AddWithValue("IdUsuario", compra.OUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProveedor", compra.OProveedor.IdProveedor);
                    cmd.Parameters.AddWithValue("Tipo_Documento", compra.TipoDocumento);
                    cmd.Parameters.AddWithValue("Numero_Dcumento", compra.NumeroDocumento);
                    cmd.Parameters.AddWithValue("Monto_Total", compra.MontoTotal);
                    cmd.Parameters.AddWithValue("Detalle_Compra", detalleCompra);
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
        public Compra GetCompra(string numero)
        {
            Compra compra = new Compra();
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select c.IdCompra,u.Nombre_Completo, p.Documento, p.Razon_Social,");
                    query.AppendLine("c.Tipo_Documento, c.Numero_Documento,c.Monto_Total, convert(char(10),c.Fecha_Registro,103)[Fecha_Registro] from COMPRA c");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = c.IdUsuario");
                    query.AppendLine("inner join PROVEEDOR p on p.IdProveedor = c.IdProveedor where c.Numero_Documento = @numero");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("numero", numero);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            compra = new Compra()
                            {
                                IdCompra = Convert.ToInt32(dr["IdCompra"]),
                                OUsuario = new Usuario() { NombreCompleto = dr["Nombre_Completo"].ToString() },
                                OProveedor = new Proveedor() { Documento = dr["Documento"].ToString(), RazonSocial = dr["Razon_Social"].ToString() },
                                TipoDocumento = dr["Tipo_Documento"].ToString(),
                                NumeroDocumento = dr["Numero_Documento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["Monto_Total"].ToString()),
                                FechaRegistro = dr["Fecha_Registro"].ToString()
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    compra = new Compra();
                }
            }
            return compra;
        }
        public List<DetalleCompra> GetDetalleCompras(int idCompra)
        {
            List<DetalleCompra> lista = new List<DetalleCompra>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select p.Nombre, dc.Precio_Compra, dc.Cantidad, dc.Monto_Total from DETALLE_COMPRA dc");
                    query.AppendLine("inner join PRODUCTO p on p.IdProducto = dc.IdProducto");
                    query.AppendLine("where dc.IdCompra = @idCompra");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("idCompra", idCompra);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader()){
                        while (dr.Read())
                        {
                            lista.Add(new DetalleCompra()
                            {
                                OProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                PrecioCompra = Convert.ToDecimal(dr["Precio_Compra"]),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                MontoTotal = Convert.ToDecimal(dr["Monto_Total"])
                            });
                        };
                    }
                }
            }catch(Exception e)
            {
                lista = new List<DetalleCompra>();
            }
            return lista;
        }
    }
}
