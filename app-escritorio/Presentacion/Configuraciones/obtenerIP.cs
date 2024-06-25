using System;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace sisgeres.Presentacion.Configuraciones
{
    public partial class obtenerIP : Form
    {
        public obtenerIP()
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
