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
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Add(new object[]
            {
                "", txtId.Text, txtDocumento.Text, txtNombreCompleto.Text, txtCorreo.Text, txtClave.Text,
                ((OpcionCombo) cbRol.SelectedItem).Valor.ToString(),
                ((OpcionCombo) cbRol.SelectedItem).Texto.ToString(),
                ((OpcionCombo) cbEstado.SelectedItem).Valor.ToString(),
                ((OpcionCombo) cbEstado.SelectedItem).Texto.ToString()
            });
        }
    }
}
