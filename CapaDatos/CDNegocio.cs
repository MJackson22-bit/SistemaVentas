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
    public class CDNegocio
    {
        public Negocio getData()
        {
            Negocio negocio = new Negocio();
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    connection.Open();
                    string query = "select IdNegocio, Nombre, RUC, Direccion from NEGOCIO where IdNegocio = 1";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            negocio = new Negocio()
                            {
                                IdNegocio = int.Parse(dr["IdNegocio"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                RUC = dr["RUC"].ToString(),
                                Direccion = dr["Direccion"].ToString()
                            };
                        }
                    }
                }
            }catch(Exception ex)
            {
                negocio = new Negocio();
            }
            return negocio;
        }
        public bool saveData(Negocio negocio, out string message)
        {
            message = string.Empty;
            bool respuesta = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    connection.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO set Nombre = @nombre,");
                    query.AppendLine("RUC = @ruc,");
                    query.AppendLine("Direccion = @direccion");
                    query.AppendLine("where IdNegocio = 1");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("@nombre", negocio.Nombre);
                    cmd.Parameters.AddWithValue("@ruc", negocio.RUC);
                    cmd.Parameters.AddWithValue("@direccion", negocio.Direccion);
                    cmd.CommandType = CommandType.Text;
                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        message = "No se pudo guardar los datos";
                        respuesta = false;
                    }
                }
            }
            catch(Exception e)
            {
                respuesta = false;
            }
            return respuesta;
        }
        public byte[] getLogo(out bool getting)
        {
            getting = true;
            byte[] logoBytes = new byte[0];
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    connection.Open();
                    string query = "select Logo from NEGOCIO where IdNegocio = 1";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            logoBytes = (byte[])dr["Logo"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                getting = false;
                logoBytes = new byte[0];
            }
            return logoBytes;
        }
        public bool updateLogo(byte[] image, out string message)
        {
            message = string.Empty;
            bool respuesta = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    connection.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO set Logo = @logo");
                    query.AppendLine("where IdNegocio = 1");
                    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
                    cmd.Parameters.AddWithValue("@logo", image);
                    cmd.CommandType = CommandType.Text;
                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        message = "No se pudo actualizar el logo";
                        respuesta = false;
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                respuesta = false;
            }
            return respuesta;
        }
    }
}
