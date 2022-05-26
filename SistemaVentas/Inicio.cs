using CapaEntidad;
using CapaNegocio;
using CapaPresentacion;
using CapaPresentacion.Modales;
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
        public Inicio(Usuario usuario = null)
        {
            if (usuario == null) usuario = new Usuario()
            {
                NombreCompleto = "Admin Predefinido",
                IdUsuario = 1
            };
            else
                userCurrent = usuario;
            userCurrent = usuario;
            InitializeComponent();
        }
        private void contenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            /*List<Permiso> listPermiso = new CNPermiso().listar(userCurrent.IdUsuario);
            foreach(IconMenuItem iconMenu in menu.Items)
            {
                bool find = listPermiso.Any(m => m.NombreMenu == iconMenu.Name);
                if(find == false)
                {
                    iconMenu.Visible = false;
                }
            }*/
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
            open_Form(menuMantenedor, new FormProducto());
        }

        private void subMenuRegistarVenta_Click(object sender, EventArgs e)
        {
            open_Form(menuVentas, new FormVenta(userCurrent));
        }

        private void subMenuDetalleVenta_Click(object sender, EventArgs e)
        {
            open_Form(menuVentas, new FormDetalleVenta());
        }

        private void subMenuDetalleCompra_Click(object sender, EventArgs e)
        {
            open_Form(menuCompras, new FormDetalleCompra());
        }

        private void subMenuRegistarCompra_Click(object sender, EventArgs e)
        {
            open_Form(menuCompras, new FormCompra(userCurrent));
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            open_Form((IconMenuItem)sender, new FormCliente());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            open_Form((IconMenuItem)sender, new FormProveedor());
        }

        private void subMenuNegocio_Click(object sender, EventArgs e)
        {
            open_Form(menuMantenedor, new FormNegocio());
        }

        private void iconMenuItem1_Click(object sender, EventArgs e)
        {
            open_Form(menuReportes, new FormReportesCompras());
        }

        private void iconMenuItem2_Click(object sender, EventArgs e)
        {
            open_Form(menuReportes, new FormReportesVentas());
        }

        private void menuReportes_Click(object sender, EventArgs e)
        {

        }

        private void menuAcerca_Click(object sender, EventArgs e)
        {
            MDAcercaDe acercaDe = new MDAcercaDe();
            acercaDe.ShowDialog();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
