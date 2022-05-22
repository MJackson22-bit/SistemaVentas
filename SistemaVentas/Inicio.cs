using CapaEntidad;
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
    }
}
