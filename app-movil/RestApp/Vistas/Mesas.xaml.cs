using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestApp.VistaModelo;
using System.Data;
using RestApp.Modelo;

namespace RestApp.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mesas : ContentPage
    {
        public Mesas()
        {
            InitializeComponent();
            dibujarSalones();
           
        }
        int idsalon=0;
        public static int idusuario;
        int idmesa;
        DataTable dtmozos;
        protected override void OnAppearing()
        {
            dibujarMesasPorSalon();
        }
        public void dibujarSalones()
        {
            DataTable dt = new DataTable();
            VMsalon funcion = new VMsalon();
            funcion.dibujarsalones(ref dt);
            foreach (DataRow rdr in dt.Rows)
            {
                Button b = new Button();
                b.Text = rdr["Salon"].ToString();
                b.CommandParameter = rdr["Id_salon"].ToString();
                b.HeightRequest = 50;
                b.WidthRequest = 150;
                b.BackgroundColor = Color.FromRgb(39, 20, 2);
                b.BorderWidth = 2;
                b.CornerRadius = 5;
                b.BorderColor= Color.FromRgb(248, 133, 18);
                b.TextColor = Color.White;
                PanelSalones.Children.Add(b);
                b.Clicked += B_Clicked;
            }
        }
        
        private void B_Clicked(object sender, EventArgs e)
        {
            idsalon = Convert.ToInt32(((Button)sender).CommandParameter);
            dibujarMesasPorSalon();
        }
        private void mostrarMozoxMesa()
        {
            var funcion = new VMmesas();
            dtmozos = new DataTable();
            funcion.mostrar_mozo_por_mesa(ref dtmozos);
        }
        public void dibujarMesasPorSalon()
        {
            mostrarMozoxMesa();
            PanelMesas.Children.Clear();
            DataTable dt = new DataTable();
            Msalon parametros = new Msalon();
            VMmesas funcion = new VMmesas();
            parametros.Id_salon = idsalon;
            funcion.dibujarMesasPorSalon(parametros, ref dt);

            foreach (DataRow rdr in dt.Rows)
            {
                string estado;
                Button b = new Button();
                Button lblmozo = new Button();
                StackLayout panel = new StackLayout();
                panel.WidthRequest = 200;
                panel.HeightRequest = 200;
                b.Text = rdr["Mesa"].ToString();
                //b.HeightRequest = 200;
                //b.WidthRequest = 200;
                b.FontSize = 40;
                b.FontAttributes = FontAttributes.Bold;
                b.BackgroundColor = Color.Transparent;
                b.VerticalOptions = LayoutOptions.FillAndExpand;

                b.CommandParameter = rdr["Id_mesa"].ToString();
                estado = rdr["Estado_de_Disponibilidad"].ToString();
                //
                //lblmozo.HeightRequest = 200;
                //lblmozo.WidthRequest = 200;
               
                if (estado == "LIBRE")
                {
                    panel.BackgroundColor = Color.FromRgb(197, 255, 145);
                    b.TextColor = Color.Black;
                }
                else
                {

                    panel.BackgroundColor = Color.FromRgb(255, 45, 54);
                    b.TextColor = Color.White;
                    lblmozo.BackgroundColor = Color.Transparent;
                    lblmozo.FontSize = 18;
                    lblmozo.FontAttributes = FontAttributes.Bold;
                    lblmozo.TextColor = Color.White;
                    lblmozo.HorizontalOptions = LayoutOptions.FillAndExpand;
                    lblmozo.CommandParameter = rdr["Id_mesa"].ToString();
                    lblmozo.Margin = new Thickness(0, 10, 0, 0);
                    lblmozo.TextTransform = TextTransform.None;
                 
                    foreach (DataRow row in dtmozos.Rows)
                    {
                        string mesa = Convert.ToString(row["Mesa"]);
                        string usuario = Convert.ToString(row["Login"]);
                        if (mesa == b.Text)
                        {
                            lblmozo.Text = usuario;
                        }
                    }
                    panel.Children.Add(lblmozo);
                }
                panel.Children.Add(b);
            

                Frame frameBoton = new Frame();
                frameBoton.Content = panel;
                frameBoton.Margin = 5;
                frameBoton.Padding = -5;
                frameBoton.HeightRequest = panel.HeightRequest;
                frameBoton.WidthRequest =panel.WidthRequest;
                frameBoton.CornerRadius = 15;
                frameBoton.HasShadow = true;
                PanelMesas.Children.Add(frameBoton);
                b.Clicked += B_Clicked1;
                lblmozo.Clicked += B_Clicked1;
            }
        }

        private void B_Clicked1(object sender, EventArgs e)
        {
            idmesa = Convert.ToInt32(((Button)sender).CommandParameter);
            Ventas.Idusuario = idusuario;
            Ventas.idmesa = idmesa;
            Navigation.PushAsync(new Ventas());
        }
    }
}