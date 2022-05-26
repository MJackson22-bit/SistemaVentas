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
                    query.AppendLine("update PRODUCTO set Stock = @cantidad where IdProducto = @idproducto");
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
    }
}
