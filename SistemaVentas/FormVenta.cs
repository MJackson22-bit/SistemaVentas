﻿using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
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
    public partial class FormVenta : Form
    {
        private Usuario usuario;
        private object txtCodigoProducto;

        public FormVenta(Usuario usuario = null)
        {
            this.usuario = usuario;
            InitializeComponent();
        }

        private void FormVenta_Load(object sender, EventArgs e)
        {
            cbTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factuta", Texto = "Factura" });
            cbTipoDocumento.DisplayMember = "Texto";
            cbTipoDocumento.ValueMember = "Valor";
            cbTipoDocumento.SelectedIndex = 0;
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProducto.Text = "0";
            txtPagaCon.Text = "";
            txtCambio.Text = "";
            txtTotalPagar.Text = "";
        }

        private void bntBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new MDCliente())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtDocCliente.Text = modal._Cliente.Documento;
                    txtNombreCliente.Text = modal._Cliente.NombreCompleto;
                    txtCodProducto.Select();
                }
                else
                {
                    txtDocCliente.Select();
                }
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new MDProducto())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtIdProducto.Text = modal._Producto.IdProducto.ToString();
                    txtCodProducto.Text = modal._Producto.Codigo;
                    txtNombreProducto.Text = modal._Producto.Nombre;
                    txtPrecioVenta.Text = modal._Producto.PrecioVenta.ToString("0.00");
                    txtStock.Text = modal._Producto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtCodProducto.Select();
                }
            }
        }

        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CNProducto().listar().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();
                if (oProducto != null)
                {
                    txtCodProducto.BackColor = Color.Honeydew;
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtNombreProducto.Text = oProducto.Nombre;
                    txtStock.Text = oProducto.Stock.ToString();
                    txtPrecioVenta.Text = oProducto.PrecioVenta.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtCodProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtNombreProducto.Text = "";
                    txtPrecioVenta.Text = "";
                    txtCantidad.Value = 1;
                    txtStock.Text = "";
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool productoExiste = false;
            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!decimal.TryParse(txtPrecioVenta.Text, out precio))
            {
                MessageBox.Show("Precio Venta - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecioVenta.Select();
                return;
            }
            if(Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Value.ToString())){
                MessageBox.Show("La cantidad no puede ser mayor al stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtStock.Select();
                return;
            }
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    productoExiste = true;
                    break;
                }
            }
            if (!productoExiste)
            {
                bool respuesta = new CNVenta().RestartStock(
                    Convert.ToInt32(txtIdProducto.Text),
                    Convert.ToInt32(txtCantidad.Value.ToString())
                    );
                if (respuesta)
                {
                    dgvData.Rows.Add(new object[]
                {
                    txtIdProducto.Text,
                    txtNombreProducto.Text,
                    precio.ToString("0.00"),
                    txtCantidad.Value.ToString(),
                    (txtCantidad.Value * precio).ToString("0.00")
                });
                    calcularTotal();
                    limpiarProducto();
                    txtCodProducto.Select();
                }
            }
        }

        private void limpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtCodProducto.BackColor = Color.White;
            txtNombreProducto.Text = "";
            txtPrecioVenta.Text = "";
            txtStock.Text = "";
            txtCantidad.Value = 1;
        }

        private void calcularTotal()
        {
            decimal total = 0;
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
            }
            txtTotalPagar.Text = total.ToString("0.00");
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.Remove_24px.Width;
                var h = Properties.Resources.Remove_24px.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.Remove_24px, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    bool respuesta = new CNVenta().SumarStock(
                        Convert.ToInt32(dgvData.Rows[indice].Cells["IdProducto"].Value.ToString()),
                        Convert.ToInt32(dgvData.Rows[indice].Cells["Cantidad"].Value.ToString())
                    );
                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(indice);
                        calcularTotal();
                    }
                   
                }
            }
        }

        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPrecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }


        private void dgvData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPrecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void btnCrearVenta_Click(object sender, EventArgs e)
        {
            if(txtDocCliente.Text == "")
            {
                MessageBox.Show("Debe ingresar documento del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(txtNombreCliente.Text == "")
            {
                MessageBox.Show("Debe el nombre del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresa productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataTable detalleVenta = new DataTable();
            detalleVenta.Columns.Add("IdProducto", typeof(int));
            detalleVenta.Columns.Add("Precio_Venta", typeof(decimal));
            detalleVenta.Columns.Add("Cantidad", typeof(int));
            detalleVenta.Columns.Add("Sub_Total", typeof(decimal));
            foreach(DataGridViewRow row in dgvData.Rows)
            {
                detalleVenta.Rows.Add(new object[]
                {
                     row.Cells["IdProducto"].Value.ToString(),
                     row.Cells["Precio"].Value.ToString(),
                     row.Cells["Cantidad"].Value.ToString(),
                     row.Cells["SubTotal"].Value.ToString(),
                });
            }
            int idCorelative = new CNVenta().getCorelative();
            string numeroDocumento = string.Format("{0:00000}", idCorelative);
            CalcularCambio();
            Venta venta = new Venta()
            {
                OUsuario = new Usuario() { IdUsuario = usuario.IdUsuario },
                TipoDocumento = ((OpcionCombo)cbTipoDocumento.SelectedItem).Texto,
                NumeroDocumento = numeroDocumento,
                DocumentoCliente = txtDocCliente.Text,
                NombreCliente = txtNombreCliente.Text,
                MontoPago = Convert.ToDecimal(txtPagaCon.Text),
                MontoCambio = Convert.ToDecimal(txtCambio.Text),
                MontoTotal = Convert.ToDecimal(txtTotalPagar.Text),
            };
            string message = string.Empty;
            bool respuesta = new CNVenta().Registar(venta, detalleVenta, out message);
            if (respuesta)
            {
                var result = MessageBox.Show("Numero de venta generada: \n" + numeroDocumento + "\n¿Desea copiar al portapapeles?", "Mensaje",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result == DialogResult.OK)
                {
                    Clipboard.SetText(numeroDocumento);
                }
                txtDocCliente.Text = "";
                txtNombreCliente.Text = "";
                dgvData.Rows.Clear();
                calcularTotal();
                txtPagaCon.Text = "";
                txtCambio.Text = "";
            }
            else
            {
                MessageBox.Show(message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtPagaCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPagaCon.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        private void CalcularCambio()
        {
            if(txtTotalPagar.Text.Trim() == "")
            {
                MessageBox.Show("No existe producto en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            decimal pagaCon;
            decimal total = Convert.ToDecimal(txtTotalPagar.Text);
            if(txtPagaCon.Text.Trim() == "")
            {
                txtPagaCon.Text = "0";
            }
            if(decimal.TryParse(txtPagaCon.Text.Trim(), out pagaCon))
            {
                if(pagaCon < total)
                {
                    txtCambio.Text = "0.00";
                }
                else
                {
                    decimal cambio = pagaCon - total;
                    txtCambio.Text = cambio.ToString("0.00");
                }
            }
        }

        private void txtPagaCon_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                CalcularCambio();
            }
        }
    }
}
