using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace VicIP
{
    public partial class Form1 : Form
    {

        List<string> listanombres = new List<string>();

        private bool loaded = false;


        public Form1()
        {
            try
            {


                InitializeComponent();
                this.TopMost = true;

                this.KeyPreview = true; // Permitir al formulario detectar las teclas antes que otros controles
                this.KeyDown += Form1_KeyDown; // Suscribir al evento KeyDown
                                               // Instanciar el objeto KeyboardListener
                KeyboardListener keyboardListener = new KeyboardListener();

                // Comenzar a escuchar las teclas
                keyboardListener.Start();


                StreamReader Leer = new("IpList.txt");
                string aux;
                string linea;
                string Name;
                string Ip;
                int pos;

                while ((linea = Leer.ReadLine()) != null)
                {
                    pos = linea.IndexOf('|');
                    aux = linea.Substring(0, pos);
                    Name = aux;
                    linea = linea.Substring(pos + 1);
                    Ip = linea;
                    string NombreIp = Name + (": ") + Ip;

                    listanombres.Add(NombreIp);
                    listboxName.DataSource = null;
                    listboxName.DataSource = listanombres;
                }
                Leer.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            this.Load += Form1_Load;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loaded = true;


            // Calcular la posición para la esquina superior derecha
            int x = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int y = 0;

            // Establecer la posición de la ventana
            this.Location = new System.Drawing.Point(x, y);

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó Ctrl+Shift+S
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                // Obtener el nombre y la IP del elemento seleccionado
                string[] partes = listboxName.SelectedItem.ToString().Split(':');
                string nombre = partes[0].Trim();
                string ip = partes[1].Trim();

                // Asignar el nombre y la IP a los cuadros de texto
                textBox1.Text = nombre;
                textBox2.Text = ip;

                // Obtener las coordenadas del cuadro de texto en Victoria 2
                GetTextBoxCoordinates(out int x, out int y);

                // Simular un clic en la ubicación del cuadro de texto
                SetCursorPos(x, y);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

                // Esperar 2 segundos para dar tiempo a que se active la ventana de Victoria 2
                Thread.Sleep(2000);

                // Enviar texto a la ventana activa
                SendKeys.Send(ip);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            string Name = textBox1.Text;
            string Ip = textBox2.Text;
            string NombreIp = (Name + ": " + Ip);
            MessageBox.Show("Se añadio: " + Name + ": " + Ip);
           
            listanombres.Add(NombreIp);
            

            //Ips.Add(Ip);

            listboxName.DataSource = null;
            listboxName.DataSource = listanombres;

            //TextWriter escribir = new StreamWriter("Datos.txt");
            //escribir.Close();
            
            foreach(string s in listanombres) 
            {
                StreamWriter agregar = File.AppendText("IpList.txt");
                agregar.WriteLine(Name + "|" + Ip);
                agregar.Close();
             }
        }
        [DllImport("USER3.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        private void del_Click(object sender, EventArgs e)
        {
            if (listboxName.SelectedItem != null && loaded)
            {
                // Obtener el nombre y la IP del elemento seleccionado
                string[] partes = listboxName.SelectedItem.ToString().Split(':');
                string nombre = partes[0].Trim();
                string ip = partes[1].Trim();

                // Asignar el nombre y la IP a los cuadros de texto
                textBox1.Text = nombre;
                textBox2.Text = ip;

                // Obtener las coordenadas del cuadro de texto en Victoria 2
                GetTextBoxCoordinates(out int x, out int y);

                // Simular un clic en la ubicación del cuadro de texto
                SetCursorPos(x, y);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

                // Esperar 1 segundo para dar tiempo a que se active la ventana de Victoria 2
                Thread.Sleep(1000);

                // Enviar texto a la ventana activa
                SendKeys.Send(ip);
            }

            

        }

        private void GetTextBoxCoordinates(out int x, out int y)
        {
            // Estas coordenadas deben ser ajustadas según la ubicación del cuadro de texto en tu pantalla
            x = 1080; // Ejemplo de coordenada X
            y = 500; // Ejemplo de coordenada Y
        }

        public void listboxName_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listboxName.SelectedItem != null && loaded)
            {
                // Obtener el nombre y la IP del elemento seleccionado
                string[] partes = listboxName.SelectedItem.ToString().Split(':');
                string nombre = partes[0].Trim();
                string ip = partes[1].Trim();

                // Asignar el nombre y la IP a los cuadros de texto
                textBox1.Text = nombre;
                textBox2.Text = ip;
                
                
            
            }


        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public int Type;
            public short Vk;
            public short Scan;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }



        private void listbut_Click(object sender, EventArgs e)
        {
            var proceso = new ProcessStartInfo("IpList.txt")
            {
                UseShellExecute = true
            };
            
            Process.Start(proceso);
        }

        private void listboxName_SelectIndex(object sender, EventArgs e)
        {
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}