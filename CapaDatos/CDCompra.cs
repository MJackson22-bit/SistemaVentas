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
    }
}
