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
    public partial class FormUsuario : Form
    {
        public FormUsuario()
        {
            InitializeComponent();
        }

        private void FormUsuario_Load(object sender, EventArgs e)
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
            List<Rol> listaRol = new CNRol().listar();
            foreach (Rol rol in listaRol)
            {
                cbRol.Items.Add(new OpcionCombo()
                {
                    Valor = rol.IdRol,
                    Texto = rol.Descripcion
                });
            }
            cbRol.DisplayMember = "Texto";
            cbRol.ValueMember = "Valor";
            cbRol.SelectedIndex = 0;

            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if(column.Visible == true && column.Name != "btnSeleccionar")
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
            List<Usuario> listaUsuario = new CNUsuario().listar();
            foreach (Usuario item in listaUsuario)
            {
                dgvData.Rows.Add(new object[]
                {
                    "", item.IdUsuario, item.Documento, item.NombreCompleto, item.Correo, item.Clave,
                    item.ORol.IdRol,
                    item.ORol.Descripcion,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                });
            }
            cbRol.DisplayMember = "Texto";
            cbRol.ValueMember = "Valor";
            cbRol.SelectedIndex = 0;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            Usuario usuario = new Usuario()
            {
                IdUsuario = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                NombreCompleto = txtNombreCompleto.Text,
                Correo = txtDocumento.Text,
                Clave = txtClave.Text,
                ORol = new Rol(){
                    IdRol = Convert.ToInt32(((OpcionCombo)cbRol.SelectedItem).Valor)

                },
                Estado = Convert.ToInt32(((OpcionCombo) cbEstado.SelectedItem).Valor) == 1 ? true : false
            };
            int idiUsuarioGenerado = new CNUsuario().Registar(usuario, out message);
            if(idiUsuarioGenerado != 0)
            {
                dgvData.Rows.Add(new object[]
                {
                    "", txtId.Text, txtDocumento.Text, txtNombreCompleto.Text, txtCorreo.Text, txtClave.Text,
                    ((OpcionCombo) cbRol.SelectedItem).Valor.ToString(),
                    ((OpcionCombo) cbRol.SelectedItem).Texto.ToString(),
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

        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtDocumento.Text = "";
            txtNombreCompleto.Text = "";
            txtCorreo.Text = "";
            txtClave.Text = "";
            txtConfirmarClave.Text = "";
            cbRol.SelectedIndex = 0;
            cbEstado.SelectedIndex = 0;
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == 0)
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
            if(dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    txtNombreCompleto.Text = dgvData.Rows[indice].Cells["NombreCompleto"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    txtClave.Text = dgvData.Rows[indice].Cells["Clave"].Value.ToString();
                    txtConfirmarClave.Text = dgvData.Rows[indice].Cells["Clave"].Value.ToString();
                    foreach (OpcionCombo opcion in cbRol.Items)
                    {
                        if(Convert.ToInt32(opcion.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdRol"].Value))
                        {
                            int indiceCombo = cbRol.Items.IndexOf(opcion);
                            cbRol.SelectedIndex = indiceCombo;
                            break;
                        }
                    }
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
    }
}
