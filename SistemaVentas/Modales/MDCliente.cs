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
    public partial class MDCliente : Form
    {
        public MDCliente()
        {
            InitializeComponent();
        }

        private void MDCliente_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if (column.Visible)
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

            //Mostrar todos los usuarios
            List<Cliente> listaCliente= new CNCliente().listar();
            foreach (Cliente item in listaCliente)
            {
                if (item.Estado)
                {
                    dgvData.Rows.Add(new object[]
                    {
                        item.Documento, item.NombreCompleto
                    });
                }
            }
        }
    }
}
