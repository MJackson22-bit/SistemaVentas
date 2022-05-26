using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FormDetalleCompra : Form
    {
        public FormDetalleCompra()
        {
            InitializeComponent();
        }

        private void bntBuscar_Click(object sender, EventArgs e)
        {
            Compra compra = new CNCompra().GetCompra(txtBuscar.Text);
            if(compra.IdCompra != 0)
            {
                txtIndice.Text = compra.NumeroDocumento;
                txtFecha.Text = compra.FechaRegistro;
                txtTipoDocumento.Text = compra.TipoDocumento;
                txtUsuario.Text = compra.OUsuario.NombreCompleto;
                txtDocProveedor.Text = compra.OProveedor.Documento;
                txtNombreProveedor.Text = compra.OProveedor.RazonSocial;
                dgvData.Rows.Clear();
                foreach(DetalleCompra dc in compra.listaDetallesCompras)
                {
                    dgvData.Rows.Add(new object[] { dc.OProducto.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal });
                }
                txtTotalPagar.Text = compra.MontoTotal.ToString("0.00");
            }
        }

        private void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocProveedor.Text = "";
            txtNombreProveedor.Text = "";
            dgvData.Rows.Clear();
            txtTotalPagar.Text = "";
        }
    }
}
