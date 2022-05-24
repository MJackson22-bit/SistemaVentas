using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNNegocio
    {
        private CDNegocio objectNeogcio = new CDNegocio();
        public Negocio getData()
        {
            return objectNeogcio.getData();
        }
        public bool saveData(Negocio negocio, out string message)
        {
            message = string.Empty;
            if (negocio.Nombre == "")
            {
                message += "Es necesario el nombre de negocio\n";
            }
            if (negocio.RUC == "")
            {
                message += "Es necesario el nombre de RUC\n";
            }
            if (negocio.Direccion == "")
            {
                message += "Es necesario la direccion\n";
            }
            if (message != string.Empty)
            {
                return false;
            }
            else
            {
                return objectNeogcio.saveData(negocio, out message);
            }
        }
        public byte[] getLogo(out bool getting)
        {
            return objectNeogcio.getLogo(out getting);
        }
        public bool updateLogo(byte[] imagen,out string message)
        {
            return objectNeogcio.updateLogo(imagen, out message);
        }
    }
}
