using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;
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
    public partial class FormReportesCompras : Form
    {
        public FormReportesCompras()
        {
            InitializeComponent();
        }

        private void FormReportesCompras_Load(object sender, EventArgs e)
        {
            List<Proveedor> lista = new CNProveedor().listar();
            cbProveedor.Items.Add(new OpcionCombo() { Valor = 0, Texto = "TODOS" });
            foreach (Proveedor item in lista)
            {
                cbProveedor.Items.Add(new OpcionCombo() { Valor = item.IdProveedor, Texto = item.RazonSocial });
            }
            cbProveedor.DisplayMember = "Texto";
            cbProveedor.ValueMember = "Valor";
            cbProveedor.SelectedIndex = 0;
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                cbBusqueda.Items.Add(new OpcionCombo()
                {
                    Valor = column.Name,
                    Texto = column.HeaderText
                });
            }
            cbBusqueda.DisplayMember = "Texto";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;
            
        }

        private void bntBuscar_Click(object sender, EventArgs e)
        {
            int idProveedor = Convert.ToInt32(((OpcionCombo)cbProveedor.SelectedItem).Valor.ToString());
            List<ReporteCompra> lista = new List<ReporteCompra>();
            lista = new CNReporte().GetCompras(
                fechaInicio.Value.ToShortDateString(),
                fechaFin.Value.ToShortDateString(),
                idProveedor
                );
            dgvData.Rows.Clear();
            foreach(ReporteCompra rc in lista)
            {
                dgvData.Rows.Add(new object[]
                {
                    rc.FechaRegistro,
                    rc.TipoDocumento,
                    rc.NumeroDocumento,
                    rc.MontoTotal,
                    rc.UsuarioRegistro,
                    rc.DocumentoProveedor,
                    rc.RazonSocial,
                    rc.CodigoProducto,
                    rc.NombreProducto,
                    rc.Categoria,
                    rc.PrecioCompra,
                    rc.PrecioVenta,
                    rc.Cantidad,
                    rc.SubTotal
                });
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();
                foreach (DataGridViewColumn column in dgvData.Columns)
                {
                    dt.Columns.Add(column.HeaderText, typeof(string));
                }
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                    {
                        dt.Rows.Add(new object[]
                        {
                            row.Cells[0].Value.ToString(),
                            row.Cells[1].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[5].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[10].Value.ToString(),
                            row.Cells[11].Value.ToString(),
                            row.Cells[12].Value.ToString(),
                            row.Cells[13].Value.ToString(),
                        });
                    }
                }
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = String.Format("ReporteCompras_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel Files | *.xlsx";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var page = wb.Worksheets.Add(dt, "Informe");
                        page.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("Reporte generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar el reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
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
