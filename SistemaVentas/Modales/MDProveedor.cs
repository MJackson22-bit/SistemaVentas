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

namespace CapaPresentacion.Modales
{
    public partial class MDProveedor : Form
    {
        public Proveedor _Proveedor { get; set; }
        public MDProveedor()
        {
            InitializeComponent();
        }

        private void MDProveedor_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if (column.Visible == true)
                {
                    cbBusqueda.Items.Add(new OpcionCombo()
                    {
                        Valor = column.Name,
                        Texto = column.HeaderText
                    });
                }
            }
            cbBusqueda.DisplayMember = "Texto";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;

            //Mostrar todos los usuarios
            List<Proveedor> listaProveedor = new CNProveedor().listar();
            foreach (Proveedor item in listaProveedor)
            {
                dgvData.Rows.Add(new object[]
                {
                    item.IdProveedor, 
                    item.Documento, 
                    item.RazonSocial,
                });
            }
        }
    }
}
