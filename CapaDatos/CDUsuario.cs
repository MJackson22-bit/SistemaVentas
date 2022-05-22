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
                    string query = "select IdUsuario, Documento, Nombre_Completo, Correo, Clave, Estado from USUARIO";
                    SqlCommand cmd = new SqlCommand(query, connection);
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
                                Estado = Convert.ToBoolean(dr["Estado"])
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
    }
}
