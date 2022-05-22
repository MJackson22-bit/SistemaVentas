using CapaEntidad;
using CapaPresentacion;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaVentas
{
    public partial class Inicio : Form
    {
        public static Usuario userCurrent;
        private static IconMenuItem menuActivo = null;
        private static Form formActivo = null;
        public Inicio(Usuario usuario)
        {
            userCurrent = usuario;
            InitializeComponent();
        }

        private void contenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = userCurrent.NombreCompleto;
        }

        private void open_Form(IconMenuItem menuItem, Form form)
        {
            if (menuActivo != null)
            {
                menuActivo.BackColor = Color.White;
            }
            menuItem.BackColor = Color.Silver;
            menuActivo = menuItem;
            if (formActivo != null)
            {
                formActivo.Close();
            }
            formActivo = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.BackColor = Color.SteelBlue;
            contenedor.Controls.Add(form);
            form.Show();
        }

        private void menuUsuario_Click(object sender, EventArgs e)
        {
            open_Form((IconMenuItem)sender, new FormUsuario());
        }

        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            open_Form(menuMantenedor, new FormCategoria());
        }

        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            open_Form(menuMantenedor, new FormCategoria());
        }

       
    }
}
