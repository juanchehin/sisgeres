﻿using sisgeres.Presentacion.Diseñocomp;
using sisgeres.Presentacion.Impresoras;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using sisgeres.Presentacion.Serializacion;
using sisgeres.Presentacion.Reportes;
using sisgeres.Presentacion.Empresaconfig;
using sisgeres.Datos;
using System.IO;
using System.Data.SqlClient;
using sisgeres.Presentacion.Licencia;
using sisgeres.Presentacion.Usuarios;
using sisgeres.Presentacion.Mesas_salones;
using sisgeres.Presentacion.PRODUCTOS;
using System.Diagnostics;
using sisgeres.libs;

namespace sisgeres.Presentacion.Configuraciones
{
    public partial class Menu_de_configuraciones : UserControl
    {
        private static LogsCustom _logsCustom = new LogsCustom();

        public Menu_de_configuraciones()
        {
            InitializeComponent();
        }
        string Servidor = @".\SQLEXPRESS";
        string txtRuta;
        string txtsoftware = "sisgeres";
        string Base_De_datos = "sisgeres";
        string ruta;
        string tipocaja;
        private void Menu_de_configuraciones_Load(object sender, EventArgs e)
        {
            centrar_panel_contenedor();
            Mostrartipocaja();
        }
        private void Mostrartipocaja()
        {
            var funcion = new Dcaja();
            var dt = new DataTable();
            funcion.mostrarCajaSerialTable(ref dt);
            tipocaja = dt.Rows[0][1].ToString();
            if (tipocaja!="PRINCIPAL")
            {
                btnReportes.Visible = false;
                btncopia.Visible = false;
                btnrestaurar.Visible = false;
            }
        }
        private void centrar_panel_contenedor()
        {
            PanelContenedor.Location = new Point((panel1.Width - PanelContenedor.Width) / 2, (panel1.Height - PanelContenedor.Height) / 2);
        }

        private void Menu_de_configuraciones_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            var frm = new PUNTO_DE_VENTA.Visor_de_mesas();
           // frm.ShowDialog();

        }

        private void btnmesas_Click(object sender, EventArgs e)
        {
            
            var frm = new Configurar_mesas_ok();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnproductos_Click(object sender, EventArgs e)
        {
            var frm = new Productos_rest();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnusuarios_Click(object sender, EventArgs e)
        {
          
            var frm = new UsuariosOk();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnImpresoras_Click(object sender, EventArgs e)
        {
            var frm = new ImpresorasConf();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnformatoticket_Click(object sender, EventArgs e)
        {
            var frm = new Diseño();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnSerie_Click(object sender, EventArgs e)
        {
            var frm = new SerialziacionComp();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            var frm = new Menureportes();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btnempresa_Click(object sender, EventArgs e)
        {
            var frm = new Eempresa();
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        private void btncopia_Click(object sender, EventArgs e)
        {
            generarCopia();
        }
        private void mostrarempresa()
        {
            var dt = new DataTable();

            Dempresa funcion = new Dempresa();
            funcion.mostrar_empresa(ref dt);
            txtRuta = dt.Rows[0][7].ToString();
        }
        private void generarCopia()
        {
            mostrarempresa();
            try
            {

                string miCarpeta = "Copias_de_Seguridad_de_" + txtsoftware;
                if (!System.IO.Directory.Exists(txtRuta + miCarpeta))

                {
                    System.IO.Directory.CreateDirectory(txtRuta + miCarpeta);
                }
                string ruta_completa = txtRuta + miCarpeta;
                string SubCarpeta = ruta_completa + @"\Respaldo_al_" + DateTime.Now.Day + "_" + (DateTime.Now.Month) + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute;
                try
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(ruta_completa, SubCarpeta));

                }
                catch (Exception ex)
                {
                    _logsCustom.alta_log("Excepcion - " + ex.Message);
                }
                try
                {
                    var funcion = new Dempresa();
                    funcion.GenerarCopiaBd(Base_De_datos, SubCarpeta);
                    MessageBox.Show("Copia de seguridad generada en: " + ruta_completa);

                }
                catch (Exception ex)
                {
                    _logsCustom.alta_log("Excepcion - " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No agregaste una ruta");
            }
        }

        private void btnrestaurar_Click(object sender, EventArgs e)
        {
            RestaurarBdExpress();
        }
        private void RestaurarBdExpress()
        {
            dlg.InitialDirectory = "";
            dlg.Filter = "Backup " + Base_De_datos + "|*.bak";
            dlg.FilterIndex = 2;
            dlg.Title = "Cargador de Backup";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ruta = Path.GetFullPath(dlg.FileName);
                DialogResult pregunta = MessageBox.Show("Usted está a punto de restaurar la base de datos," + "asegurese de que el archivo .bak sea reciente, de" + "lo contrario podría perder información y no podrá" + "recuperarla, ¿desea continuar?", "Restauración de base de datos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (pregunta == DialogResult.Yes)
                {
                    SqlConnection cnn = new SqlConnection("Server=" + Servidor + ";database=master; integrated security=yes");
                    try
                    {
                        cnn.Open();
                        string Proceso = "EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'" + Base_De_datos + "' USE [master] ALTER DATABASE [" + Base_De_datos + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE DROP DATABASE [" + Base_De_datos + "] RESTORE DATABASE " + Base_De_datos + " FROM DISK = N'" + ruta + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10";
                        SqlCommand BorraRestaura = new SqlCommand(Proceso, cnn);
                        BorraRestaura.ExecuteNonQuery();
                        ReemplazarSerialPc();
                        MessageBox.Show("La base de datos ha sido restaurada satisfactoriamente! Vuelve a Iniciar El Aplicativo", "Restauración de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();


                    }
                    catch (Exception ex)
                    {
                        _logsCustom.alta_log("Excepcion - " + ex.Message);
                        RestaurarNoExpress();
                    }
                    finally
                    {
                        if (cnn.State == ConnectionState.Open)
                        {
                            cnn.Close();
                        }

                    }


                }
            }
        }
        private void ReemplazarSerialPc()
        {
            var funcion = new Dcaja();
            funcion.ReemplazarSerialPc();
        }
        private void RestaurarNoExpress()
        {
            Servidor = ".";
            SqlConnection cnn = new SqlConnection("Server=" + Servidor + ";database=master; integrated security=yes");
            try
            {
                cnn.Open();
                string Proceso = "EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'" + Base_De_datos + "' USE [master] ALTER DATABASE [" + Base_De_datos + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE DROP DATABASE [" + Base_De_datos + "] RESTORE DATABASE " + Base_De_datos + " FROM DISK = N'" + ruta + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10";
                SqlCommand BorraRestaura = new SqlCommand(Proceso, cnn);
                BorraRestaura.ExecuteNonQuery();
                ReemplazarSerialPc();
                MessageBox.Show("La base de datos ha sido restaurada satisfactoriamente! Vuelve a Iniciar El Aplicativo", "Restauración de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();


            }
            catch (Exception ex)
            {
                _logsCustom.alta_log("Excepcion - " + ex.Message);
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ctl = new Licencias();
            ctl.Dock = DockStyle.Fill;
            ctl.btnCerrar.Visible = true;
            panel1.Controls.Add(ctl);
            ctl.BringToFront();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            var ctl = new Clientesconfig();
            ctl.Dock = DockStyle.Fill;
         //   ctl.btnCerrar.Visible = true;
            panel1.Controls.Add(ctl);
            ctl.BringToFront();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btn_open_logs_Click(object sender, EventArgs e)
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

        private void btn_obtenerIP_Click(object sender, EventArgs e)
        {
            obtenerIP frm = new obtenerIP();
            frm.Show();
        }
    }
}
