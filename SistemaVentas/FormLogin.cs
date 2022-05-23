using CapaEntidad;
using CapaNegocio;
using SistemaVentas;
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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            Usuario usuario = new CNUsuario().listar().Where(u => u.Documento == txtDocumento.Text && u.Clave == txtClave.Text).FirstOrDefault();
            if(usuario != null)
            {
                Inicio inicio = new Inicio(usuario);
                inicio.Show();
                this.Hide();
                inicio.FormClosing += form_Closing;
            }
            else
            {
                MessageBox.Show("No se encontró el usuario", "Ha ocurrido un problema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void form_Closing(object sender, FormClosingEventArgs e)
        {
            this.Show();
            txtClave.Text = "";
            txtDocumento.Text = "";
        }
    }
}
