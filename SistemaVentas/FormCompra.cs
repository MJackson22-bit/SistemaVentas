using CapaEntidad;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
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
    public partial class FormCompra : Form
    {
        private Usuario usuario;
        public FormCompra(Usuario usuario = null)
        {
            this.usuario = usuario;
            InitializeComponent();
        }

        private void bntBuscar_Click(object sender, EventArgs e)
        {
            using(var modal = new MDProveedor())
            {
                var result = modal.ShowDialog();
            }
        }

        private void FormCompra_Load(object sender, EventArgs e)
        {
            cbTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factuta", Texto = "Factura" });
            cbTipoDocumento.DisplayMember = "Texto";
            cbTipoDocumento.ValueMember = "Valor";
            cbTipoDocumento.SelectedIndex = 0;
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProducto.Text = "0";
            txtIdProveedor.Text = "0";
        }
    }
}
