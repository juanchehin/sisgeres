using sisgeres.libs;
using sisgeres.Presentacion.Conexionremota;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace sisgeres.Presentacion.AsistenteInstalacion
{
    public partial class EleccionServidor : Form
    {
        private static LogsCustom _logsCustom = new LogsCustom();

        public EleccionServidor()
        {
            InitializeComponent();
        }
        int Idusuario;
        private Logica.AES aes = new Logica.AES();

        private void EleccionServidor_Load(object sender, EventArgs e)
        {
            PanelPc.Location = new Point(Convert.ToInt32((Width - PanelPc.Width) / 2.0), Convert.ToInt32((Height - PanelPc.Height) / 2.0));
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            PanelServidor.Visible = false;
            PanelPc.Visible = true;
        }

        private void btnconectar_Click(object sender, EventArgs e)
        {
            if (panelUsuario.Visible == false)
            {
                txtCadena.Text = "Data Source=" + txtservidor.Text + ";Initial Catalog=" + txtBd.Text + ";Integrated Security=True";
                probarconexion();
            }
            else
            {
                txtCadena.Text = "Data Source=" + txtservidor.Text + ";Initial Catalog=" + txtBd.Text + ";Integrated Security=False; " + "User Id=" + txtusuario.Text + ";Password=" + txtcontraseña.Text;
                probarconexion();
            }
        }
        private void probarconexion()
        {
            try
            {
                SqlConnection conexionManual = new SqlConnection(txtCadena.Text);
                conexionManual.Open();
                SqlCommand cmd = new SqlCommand("Select Top 1 IdUsuario from Usuarios", conexionManual);
                Idusuario = Convert.ToInt32(cmd.ExecuteScalar());
                conexionManual.Close();
                SavetoXML(aes.Encrypt(txtCadena.Text, Logica.Desencryptacion.appPwdUnique, int.Parse("256")));
                MessageBox.Show("!Listo! - vuelve a abrir el sistema");
                Dispose();
            }
            catch (Exception ex)
            {
                _logsCustom.alta_log("Excepcion EleccionServidor.cs - probarconexion - " + ex.Message);

                Idusuario = 0;
                MessageBox.Show("sin conexion");
            }
          
        }
        public void SavetoXML(object dbcnString)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("ConnectionString.xml");
                XmlElement root = doc.DocumentElement;
                root.Attributes[0].Value = Convert.ToString(dbcnString);
                XmlTextWriter writer = new XmlTextWriter("ConnectionString.xml", null);
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                _logsCustom.alta_log("Excepcion EleccionServidor.cs - SavetoXML - " + ex.Message);
                MessageBox.Show("Ocurrio un error. Contactese con el administrador");
            }
        }
        private void checkUsuario_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUsuario.Checked == true)
            {
                panelUsuario.Visible = true;
            }
            else
            {
                panelUsuario.Visible = false;

            }
        }

        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            Dispose();
            var frm = new InstalarServidor();
            frm.ShowDialog();
        }

        private void btnServidorlisto_Click(object sender, EventArgs e)
        {
            PanelServidor.Visible = true;
            PanelPc.Visible = false;
            PanelServidor.Location = new Point(Convert.ToInt32((Width - PanelServidor.Width) / 2.0), Convert.ToInt32((Height - PanelServidor.Height) / 2.0));
        }

        private void btnSecundaria_Click(object sender, EventArgs e)
        {
            Dispose();
            var frm = new Asistenteconexion();
            frm.ShowDialog();
        }

        private void btnAbrirLogs_Click(object sender, EventArgs e)
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string logsFolder = Path.Combine(appDataFolder, "sisgeres");

            // Verifica si el directorio existe
            if (Directory.Exists(logsFolder))
            {
                // Abre el directorio en el Explorador de Windows
                Process.Start("explorer.exe", logsFolder);
            }
            else
            {
                MessageBox.Show("La carpeta de logs no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
