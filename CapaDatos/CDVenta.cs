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
    }
}
