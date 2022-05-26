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
    public partial class FormDetalleCompra : Form
    {
        public FormDetalleCompra()
        {
            InitializeComponent();
        }

        private void bntBuscar_Click(object sender, EventArgs e)
        {
            Compra compra = new CNCompra().GetCompra(txtBuscar.Text);
            if(compra.IdCompra != 0)
            {
                txtIndice.Text = compra.NumeroDocumento;
                txtFecha.Text = compra.FechaRegistro;
                txtTipoDocumento.Text = compra.TipoDocumento;
                txtUsuario.Text = compra.OUsuario.NombreCompleto;
                txtDocProveedor.Text = compra.OProveedor.Documento;
                txtNombreProveedor.Text = compra.OProveedor.RazonSocial;
                dgvData.Rows.Clear();
                foreach(DetalleCompra dc in compra.listaDetallesCompras)
                {
                    dgvData.Rows.Add(new object[] { dc.OProducto.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal });
                }
                txtTotalPagar.Text = compra.MontoTotal.ToString("0.00");
            }
        }

        private void btnLimpiarBuscar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocProveedor.Text = "";
            txtNombreProveedor.Text = "";
            dgvData.Rows.Clear();
            txtTotalPagar.Text = "";
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if(txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string textoHtml = Properties.Resources.PlantillaCompra.ToString();
            Negocio negocio = new CNNegocio().getData();
            textoHtml = textoHtml.Replace("@nombrenegocio", negocio.Nombre.ToUpper());
            textoHtml = textoHtml.Replace("@docnegocio", negocio.RUC);
            textoHtml = textoHtml.Replace("@direcnegocio", negocio.Direccion);
            textoHtml = textoHtml.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            textoHtml = textoHtml.Replace("@numerodocumento", txtIndice.Text);
            textoHtml = textoHtml.Replace("@nombrenegocio", negocio.Nombre.ToUpper());
            textoHtml = textoHtml.Replace("@docproveedor", txtDocProveedor.Text);
            textoHtml = textoHtml.Replace("@nombreproveedor", txtNombreProveedor.Text);
            textoHtml = textoHtml.Replace("@fecharegistro", txtFecha.Text);
            textoHtml = textoHtml.Replace("@usuarioregistro", txtUsuario.Text);
            string rows = string.Empty;
            foreach(DataGridViewRow row in dgvData.Rows)
            {
                rows += "<tr>";
                rows += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                rows += "<td>" + row.Cells["PrecioCompra"].Value.ToString() + "</td>";
                rows += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                rows += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                rows += "</tr>";
            }
            textoHtml = textoHtml.Replace("@filas", rows);
            textoHtml = textoHtml.Replace("@montototal", txtTotalPagar.Text);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = String.Format("Compra_{0}.pdf", txtIndice.Text);
            saveFile.Filter = "Excel Files | *.pdf";
            if(saveFile.ShowDialog() == DialogResult.OK)
            {
                using(FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
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
                    using(StringReader sr = new StringReader(textoHtml))
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
