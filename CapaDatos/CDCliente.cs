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
    public class CDCliente
    {
        public List<Cliente> listar()
        {
            List<Cliente> lista = new List<Cliente>();
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdCliente, Documento, Nombre_Completo, Correo, Telefono, Estado from CLIENTE");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["Nombre_Completo"].ToString(),
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

        public int Registrar(Cliente cliente, out string message)
        {
            int idiClienteGenerado = 0;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCLIENTE", connection);
                    cmd.Parameters.AddWithValue("Documento", cliente.Documento);
                    cmd.Parameters.AddWithValue("Nombre_Completo", cliente.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("Telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("Estado", cliente.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    idiClienteGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    message = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                idiClienteGenerado = 0;
                message = e.Message;
            }
            return idiClienteGenerado;
        }

        public bool Editar(Cliente cliente, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_MODIFICARCLIENTE", connection);
                    cmd.Parameters.AddWithValue("IdCliente", cliente.IdCliente);
                    cmd.Parameters.AddWithValue("Documento", cliente.Documento);
                    cmd.Parameters.AddWithValue("Nombre_Completo", cliente.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("Telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("Estado", cliente.Estado);
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


        public bool Eliminar(Cliente cliente, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("delete from CLIENTE where IdCliente = @id", connection);
                    cmd.Parameters.AddWithValue("id", cliente.IdCliente);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
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
