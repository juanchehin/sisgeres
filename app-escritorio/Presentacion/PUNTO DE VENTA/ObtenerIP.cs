using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sisgeres.Presentacion.PUNTO_DE_VENTA
{
    public partial class ObtenerIP : Form
    {
        public ObtenerIP()
        {
            InitializeComponent();
            obtenerIp();
        }

        private void obtenerIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.lblIP.Text = ip.ToString();
                }
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
