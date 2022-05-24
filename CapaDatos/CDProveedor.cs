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
    public class CDProveedor
    {
        public List<Proveedor> listar()
        {
            List<Proveedor> lista = new List<Proveedor>();
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdProveedor, Documento, Razon_Social, Correo, Telefono, Estado from PROVEEDOR");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Proveedor()
                            {
                                IdProveedor = Convert.ToInt32(dr["IdProveedor"]),
                                Documento = dr["Documento"].ToString(),
                                RazonSocial = dr["Razon_Social"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    lista = null;
                }
            }
            return lista;
        }

        public int Registrar(Proveedor proveedor, out string message)
        {
            int idiProveedorGenerado = 0;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARPROVEEDOR", connection);
                    cmd.Parameters.AddWithValue("Documento", proveedor.Documento);
                    cmd.Parameters.AddWithValue("Razon_Social", proveedor.RazonSocial);
                    cmd.Parameters.AddWithValue("Correo", proveedor.Correo);
                    cmd.Parameters.AddWithValue("Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("Estado", proveedor.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    idiProveedorGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    message = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                idiProveedorGenerado = 0;
                message = e.Message;
            }
            return idiProveedorGenerado;
        }

        public bool Editar(Proveedor proveedor, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_MODIFICARPROVEEDOR", connection);
                    cmd.Parameters.AddWithValue("IdProveedor", proveedor.IdProveedor);
                    cmd.Parameters.AddWithValue("Documento", proveedor.Documento);
                    cmd.Parameters.AddWithValue("Razon_Social", proveedor.RazonSocial);
                    cmd.Parameters.AddWithValue("Correo", proveedor.Correo);
                    cmd.Parameters.AddWithValue("Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("Estado", proveedor.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    message = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                respuesta = false;
                message = e.Message;
            }
            return respuesta;
        }


        public bool Eliminar(Proveedor proveedor, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARPROVEEDOR", connection);
                    cmd.Parameters.AddWithValue("IdProveedor", proveedor.IdProveedor);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    message = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                respuesta = false;
                message = e.Message;
            }
            return respuesta;
        }
    }
}
