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
    }
}
