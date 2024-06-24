using System;
using System.Data.SqlClient;
using System.Data;

namespace sisgeres.Datos
{

    class CONEXIONMAESTRA
    {
        public static string conexion = Convert.ToString(Logica.Desencryptacion.checkServer());
        public static SqlConnection conectar = new SqlConnection(conexion);

        public static  void abrir()
        {

            if (conectar.State == ConnectionState.Closed  )
            {
                conectar.Open();
            }

        }
        public static void cerrar()
        {
            if(conectar.State == ConnectionState.Open )
            {
                conectar.Close();
            }
        }
    }
}
