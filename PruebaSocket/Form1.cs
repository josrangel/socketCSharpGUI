using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;               
using System.Net.Sockets;  

namespace PruebaSocket
{
    public partial class Form1 : Form
    {
        ThreadStart delegado;
        Socket miPrimerSocket;
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; 
            textBoxIP.Text = IPAddress.Any.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEscuchar_Click(object sender, EventArgs e)
        {
            delegado = new ThreadStart(Conectar);
            Thread hilo = new Thread(delegado);
            //Iniciamos el hilo 
            hilo.Start(); 
        }

        public void Conectar()
        {
            String iptxt= textBoxIP.Text;
            int puertotxt= int.Parse(textBoxPuerto.Text);
            byte[] ByRec;
            miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse(iptxt), puertotxt);

            try
            {
                miPrimerSocket.Bind(miDireccion);
                miPrimerSocket.Listen(1);

                textBoxRespuesta.Text += "Escuchando..." + "\r\n";
                Socket Escuchar = miPrimerSocket.Accept();
                textBoxRespuesta.Text += "Conectado..." + "\r\n";
                
                while (true)
                {
                    ByRec = new byte[255];
                    int a = Escuchar.Receive(ByRec, 0, ByRec.Length, 0);
                    Array.Resize(ref ByRec, a);
                    textBoxRespuesta.Text += "Cliente: " + Encoding.Default.GetString(ByRec)  + "\r\n"; //mostramos lo recibido
                }

            }
            catch (Exception error)
            {
                textBoxRespuesta.Text +="Error: "+ error.ToString();
            }finally{
                miPrimerSocket.Close();
            }           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            miPrimerSocket.Close();
        }
    }
}