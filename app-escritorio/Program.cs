using sisgeres.Presentacion.Login;
using System;
using System.Windows.Forms;

namespace sisgeres
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var frm = new LoginForm();
            frm.FormClosed += frm_closed;
            frm.ShowDialog();
            Application.Run();
        }
        private static void frm_closed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }
    }
}
