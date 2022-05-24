using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FormNegocio : Form
    {
        public FormNegocio()
        {
            InitializeComponent();
        }
        public Image ByteImage(byte[] imageBytes)
        {
            MemoryStream memory = new MemoryStream();
            memory.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(memory);
            return image;
        }

        private void FormNegocio_Load(object sender, EventArgs e)
        {
            bool getting = true;
            byte[] byteImage = new CNNegocio().getLogo(out getting);
            if (getting)
                pictureLogo.Image = ByteImage(byteImage);
            Negocio negocio = new CNNegocio().getData();
            txtNombre.Text = negocio.Nombre;
            txtRUC.Text = negocio.RUC;
            txtDireccion.Text = negocio.Direccion;
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.FileName = "File|*.jpg;*.jpeg;*.png";
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                byte[] byteImage = File.ReadAllBytes(openFile.FileName);
                bool respuesta = new CNNegocio().updateLogo(byteImage, out message);
                if (respuesta)
                    pictureLogo.Image = ByteImage(byteImage);
                else
                    MessageBox.Show(message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string messaje = string.Empty;
            Negocio negocio = new Negocio()
            {
                Nombre = txtNombre.Text,
                RUC = txtRUC.Text,
                Direccion = txtDireccion.Text
            };
            bool respuesta = new CNNegocio().saveData(negocio, out messaje);
            if (respuesta)
                MessageBox.Show("Los cambios fueron guardados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se pudo guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
