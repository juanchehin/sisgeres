﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sisgeres.Presentacion.PUNTO_DE_VENTA
{
    public partial class NombreLlevar : Form
    {
        public NombreLlevar()
        {
            InitializeComponent();
        }
        char[] alfabeto;
        char[] numeros;
        public static int idventa;
        public static string nombre;
        private void agregarNumeros()
        {
            numeros = "0123456789".ToCharArray();
            foreach (char numer in numeros)
            {
                Button btnnumero = new Button();
                btnnumero.Text = numer.ToString();
                btnnumero.BackgroundImage = Properties.Resources.negro;
                btnnumero.BackgroundImageLayout = ImageLayout.Stretch;
                btnnumero.BackColor = Color.Transparent;
                btnnumero.FlatStyle = FlatStyle.Flat;
                btnnumero.FlatAppearance.BorderSize = 0;
                btnnumero.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnnumero.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnnumero.Size = new Size(55, 55);
                btnnumero.ForeColor = Color.White;
                btnnumero.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                PanelNumeros.Controls.Add(btnnumero);
                btnnumero.Click += Btnnumero_Click;

            }
        }
        private void agregarLetras()
        {
            alfabeto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ".ToCharArray();
            foreach (char Letra in alfabeto)
            {

                Button btnletra = new Button();
                btnletra.Text = Letra.ToString();
                btnletra.BackgroundImage = Properties.Resources.naranja;
                btnletra.BackgroundImageLayout = ImageLayout.Stretch;
                btnletra.BackColor = Color.Transparent;
                btnletra.FlatStyle = FlatStyle.Flat;
                btnletra.FlatAppearance.BorderSize = 0;
                btnletra.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnletra.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnletra.Size = new Size(55, 55);
                btnletra.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                PanelLetras.Controls.Add(btnletra);
                btnletra.Click += Btnletra_Click;
            }


        }
        private void Btnletra_Click(object sender, EventArgs e)
        {
            var letra = ((Button)sender).Text;
            txtnombre.Text += letra;
        }

        private void Btnnumero_Click(object sender, EventArgs e)
        {
            var numero = ((Button)sender).Text;
            txtnombre.Text += numero;
        }

        private void NombreLlevar_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            agregarNumeros();
            agregarLetras();
            txtnombre.Text = nombre;
            txtnombre.SelectAll();
        }

        private void btnBorrarCaract_Click(object sender, EventArgs e)
        {
            int contador;
            contador = txtnombre.Text.Count();
            if (contador > 0)
            {

                txtnombre.Text = txtnombre.Text.Substring(0, txtnombre.Text.Count() - 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtnombre.Text += " ";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtnombre.Text))
            {        
                Dispose();
            }
            else
            {
                MessageBox.Show("Ingrese un nombre");

            }
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtnombre.Text))
            {
                Punto_de_venta.nombrellevar = txtnombre.Text;
                Dispose();
            }
            else
            {
                MessageBox.Show("Ingrese un nombre");

            }
        }
    }
}
