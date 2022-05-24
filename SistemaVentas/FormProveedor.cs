using CapaEntidad;
using CapaNegocio;
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
    public partial class FormProveedor : Form
    {
        public FormProveedor()
        {
            InitializeComponent();
        }

        private void FormProveedor_Load(object sender, EventArgs e)
        {
            cbEstado.Items.Add(new OpcionCombo()
            {
                Valor = 1,
                Texto = "Activo"
            });
            cbEstado.Items.Add(new OpcionCombo()
            {
                Valor = 0,
                Texto = "No activo"
            });
            cbEstado.DisplayMember = "Texto";
            cbEstado.ValueMember = "Valor";
            cbEstado.SelectedIndex = 0;

            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if (column.Visible == true && column.Name != "btnSeleccionar")
                {
                    cbBusqueda.Items.Add(new OpcionCombo()
                    {
                        Valor = column.Name,
                        Texto = column.HeaderText
                    });
                }
            }
            cbBusqueda.DisplayMember = "Texto";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;

            //Mostrar todos los usuarios
            List<Proveedor> listaUsuario = new CNProveedor().listar();
            foreach (Proveedor item in listaUsuario)
            {
                dgvData.Rows.Add(new object[]
                {
                    "", item.IdProveedor, item.Documento, item.RazonSocial, item.Correo, item.Telefono,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            Proveedor proveedor = new Proveedor()
            {
                IdProveedor = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                RazonSocial = txtRazonSocial.Text,
                Correo = txtDocumento.Text,
                Telefono = txtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false
            };
            if (proveedor.IdProveedor == 0)
            {
                int idGenerado = new CNProveedor().Registar(proveedor, out message);
                if (idGenerado != 0)
                {
                    dgvData.Rows.Add(new object[]
                    {
                    "", idGenerado, txtDocumento.Text, txtRazonSocial.Text, txtCorreo.Text, txtTelefono.Text,
                    ((OpcionCombo) cbEstado.SelectedItem).Valor.ToString(),
                    ((OpcionCombo) cbEstado.SelectedItem).Texto.ToString()
                    });
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
            else
            {
                bool resultado = new CNProveedor().Editar(proveedor, out message);
                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Documento"].Value = txtDocumento.Text;
                    row.Cells["RazonSocial"].Value = txtRazonSocial.Text;
                    row.Cells["Correo"].Value = txtCorreo.Text;
                    row.Cells["Telefono"].Value = txtTelefono.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cbEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cbEstado.SelectedItem).Texto.ToString();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(message);
                }

            }
        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtDocumento.Text = "";
            txtRazonSocial.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            cbEstado.SelectedIndex = 0;
            txtDocumento.Select();
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.Check_Circle_24px.Width;
                var h = Properties.Resources.Check_Circle_24px.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.Check_Circle_24px, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    txtRazonSocial.Text = dgvData.Rows[indice].Cells["RazonSocial"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    txtTelefono.Text = dgvData.Rows[indice].Cells["Telefono"].Value.ToString();
                   
                    foreach (OpcionCombo opcion in cbEstado.Items)
                    {
                        if (Convert.ToInt32(opcion.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indiceCombo = cbEstado.Items.IndexOf(opcion);
                            cbEstado.SelectedIndex = indiceCombo;
                            break;
                        }
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar el proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string message = string.Empty;
                    Proveedor proveedor = new Proveedor()
                    {
                        IdProveedor = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = new CNProveedor().Eliminar(proveedor, out message);
                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void bntBuscar_Click(object sender, EventArgs e)
        {
            string columnFilter = ((OpcionCombo)cbBusqueda.SelectedItem).Valor.ToString();
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[columnFilter].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
}
