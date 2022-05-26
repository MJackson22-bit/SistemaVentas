using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
    public partial class FormDetalleVenta : Form
    {
        public FormDetalleVenta()
        {
            InitializeComponent();
        }

        private void bntBuscar_Click(object sender, EventArgs e)
        {
            Venta venta = new CNVenta().GetVenta(txtBuscar.Text);
            if (venta.IdVenta != 0)
            {
                txtIndice.Text = venta.NumeroDocumento;
                txtFecha.Text = venta.FechaRegistro;
                txtTipoDocumento.Text = venta.TipoDocumento;
                txtUsuario.Text = venta.OUsuario.NombreCompleto;
                txtDocCliente.Text = venta.DocumentoCliente;
                txtNombreCLiente.Text = venta.NombreCliente;
                dgvData.Rows.Clear();
                foreach (DetalleVenta dc in venta.ListDetalleVenta)
                {
                    dgvData.Rows.Add(new object[] { dc.OProducto.Nombre, dc.PrecioVenta, dc.Cantidad, dc.SubTotal});
                }
                txtMontoTotal.Text = venta.MontoTotal.ToString("0.00");
                txtMontoPago.Text = venta.MontoPago.ToString("0.00");
                txtMontoCambio.Text = venta.MontoCambio.ToString("0.00");
            }
        }

        private void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocCliente.Text = "";
            txtNombreCLiente.Text = "";
            dgvData.Rows.Clear();
            txtMontoCambio.Text = "";
            txtMontoTotal.Text = "";
            txtMontoPago.Text = "";
        }

        private void FormDetalleVenta_Load(object sender, EventArgs e)
        {
            txtBuscar.Select();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string textoHtml = Properties.Resources.PlantillaVenta.ToString();
            Negocio negocio = new CNNegocio().getData();
            textoHtml = textoHtml.Replace("@nombrenegocio", negocio.Nombre.ToUpper());
            textoHtml = textoHtml.Replace("@docnegocio", negocio.RUC);
            textoHtml = textoHtml.Replace("@direcnegocio", negocio.Direccion);
            textoHtml = textoHtml.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            textoHtml = textoHtml.Replace("@numerodocumento", txtIndice.Text);
            textoHtml = textoHtml.Replace("@doccliente", txtDocCliente.Text);
            textoHtml = textoHtml.Replace("@nombrecliente", txtNombreCLiente.Text);
            textoHtml = textoHtml.Replace("@nombrenegocio", negocio.Nombre.ToUpper());
            textoHtml = textoHtml.Replace("@fecharegistro", txtFecha.Text);
            textoHtml = textoHtml.Replace("@usuarioregistro", txtUsuario.Text);
            string rows = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                rows += "<tr>";
                rows += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                rows += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                rows += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                rows += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                rows += "</tr>";
            }
            textoHtml = textoHtml.Replace("@filas", rows);
            textoHtml = textoHtml.Replace("@montototal", txtMontoTotal.Text);
            textoHtml = textoHtml.Replace("@pagocon", txtMontoPago.Text);
            textoHtml = textoHtml.Replace("@cambio", txtMontoCambio.Text);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = String.Format("Venta_{0}.pdf", txtIndice.Text);
            saveFile.Filter = "PDF Files | *.pdf";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    bool obtenido = true;
                    byte[] byteImage = new CNNegocio().getLogo(out obtenido);
                    if (obtenido)
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(byteImage);
                        image.ScaleToFit(60, 60);
                        image.Alignment = iTextSharp.text.Image.UNDERLYING;
                        image.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(image);
                    }
                    using (StringReader sr = new StringReader(textoHtml))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }
                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
