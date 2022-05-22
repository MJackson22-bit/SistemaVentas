using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CDUsuario
    {
        public List<Usuario> listar()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select u.IdUsuario, u.Documento, u.Nombre_Completo, u.Correo, u.Clave, u.Estado, r.IdRol, r.Descripcion from USUARIO u");
                    query.AppendLine("inner join Rol r on r.IdRol = u.IdRol");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["Nombre_Completo"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                ORol = new Rol()
                                {
                                    IdRol = Convert.ToInt32(dr["IdRol"]),
                                    Descripcion = dr["Descripcion"].ToString()
                                }
                            });
                        }
                    }
                }catch(Exception e)
                {
                    lista = null;
                }
            }
            return lista;
        }

        public int Registrar(Usuario usuario, out string message)
        {
            int idiUsuarioGenerado = 0;
            message = string.Empty;
            try
            {
                using(SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", connection);
                    cmd.Parameters.AddWithValue("Documento", usuario.Documento);
                    cmd.Parameters.AddWithValue("Nombre_Completo", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("IdRol", usuario.ORol.IdRol);
                    cmd.Parameters.AddWithValue("Esatdo", usuario.Estado);
                    cmd.Parameters.AddWithValue("IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    idiUsuarioGenerado = Convert.ToInt32(cmd.Parameters["IdUsuarioResultado"].Value);
                    message = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }catch(Exception e)
            {
                idiUsuarioGenerado = 0;
                message = e.Message;
            }
            return idiUsuarioGenerado;
        }

        public bool Editar(Usuario usuario, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", connection);
                    cmd.Parameters.AddWithValue("IdUsuario", usuario.IdUsuario);
                    cmd.Parameters.AddWithValue("Documento", usuario.Documento);
                    cmd.Parameters.AddWithValue("Nombre_Completo", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("IdRol", usuario.ORol.IdRol);
                    cmd.Parameters.AddWithValue("Esatdo", usuario.Estado);
                    cmd.Parameters.AddWithValue("IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
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


        public bool Eliminar(Usuario usuario, out string message)
        {
            bool respuesta = false;
            message = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", connection);
                    cmd.Parameters.AddWithValue("IdUsuario", usuario.IdUsuario);
                    cmd.Parameters.AddWithValue("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
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
