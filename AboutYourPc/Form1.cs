using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;
using System.Management;
using System.IO;

namespace AboutYourPc
{

    public partial class Form1 : Form
    {

        private bool FormMove = false;
        private Point StartPoint = new Point(0, 0);              // for move
        
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        PerformanceCounter perSystemCounter = new PerformanceCounter("System", "System up time");
        PerformanceCounter MemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
                
        string UserName = Environment.UserName;            // получает имя пользователя                                                                                               
        bool Bit64 = Environment.Is64BitOperatingSystem;   // язывляется ли система 64 битная                                              
        string MachineName = Environment.MachineName;      // машинное имя                                                                                                                                 
        string Directory = Environment.SystemDirectory;    // Получает полный путь к системному каталогу                                           
        string BitPC;
        object CpuName, GpuName, WindowsName, Users, RamName, Threads, RamNameTwo, TotalRam, MaxRam, windowsVersion;

        public Form1()
        {

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            ManagementObjectSearcher mos =
 new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");                    // Processor name 
            foreach (ManagementObject mo in mos.Get())
            {
                CpuName = mo["Name"];
                Threads = mo["NumberOfCores"] + "/" + mo["NumberOfLogicalProcessors"];   // CPU Threads name 
            }
            ManagementObjectSearcher video =
new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");               // VideoCard name 
            foreach (ManagementObject mo in video.Get())
            {
                GpuName = mo["Name"];
            }
            ManagementObjectSearcher windows =
new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_OperatingSystem");                  // Windows name 
            foreach (ManagementObject mo in windows.Get())
            {
                WindowsName = mo["Caption"];
                windowsVersion =  mo["Version"];
                Users = mo["NumberOfUsers"];                           //How many users name 
            }

            ManagementObjectSearcher ramm =
new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");                  // RAM Mhz and devided quantityname 
            foreach (ManagementObject mo in ramm.Get())
            {
                RamName += "\r\nRAM: " + Convert.ToDouble(mo.GetPropertyValue("Capacity")) / 1073741824 + "GB" +" "+ Convert.ToDouble(mo.GetPropertyValue("Speed")) + " Mhz" + " . Model: "+(mo.GetPropertyValue("PartNumber"));
                RamNameTwo += "\r\nRAM: " + Convert.ToDouble(mo.GetPropertyValue("Capacity")) / 1073741824 + "GB" + " " + Convert.ToDouble(mo.GetPropertyValue("Speed")) + " Mhz";
                MaxRam = mo["TotalWidth"];                         // Maximum RAM can use
            }

            ManagementObjectSearcher Upgrade =
new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");                  // Total Ram Quantity name 
            foreach (ManagementObject mo in Upgrade.Get())
            {
                TotalRam = "" + Convert.ToInt64(mo.GetPropertyValue("TotalPhysicalMemory")) / 1073741824; 
            }
        }


       // async void Method()
        //{
          // await Task.Run(() =>              // async
          //  {

          //  });
        //}


        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = UserName;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = Bit64 ? "64Bit" : "32Bit";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox4.Text = MachineName;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            textBox3.Text = "" + Threads + " Cores";                  
        }
        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Text ="" + WindowsName;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            textBox7.Text = ""+Users;
        }
        private void button8_Click(object sender, EventArgs e)            // page size Bit . add button and textbox if needed
        {
            textBox9.Text = "" + TotalRam + " Gb";
        }
        private void button10_Click(object sender, EventArgs e)
        {
            textBox6.Text = "" + MaxRam + " Gb";
        }
        private void button12_Click(object sender, EventArgs e)    // if we need descriptors, add button and textbox
        {
            textBox8.Text = "" + CpuName;
       }
        private void button13_Click(object sender, EventArgs e)
        {
            var t = this.Controls.OfType<TextBox>().AsEnumerable<TextBox>();   // clear all texboxes
            foreach (TextBox item in t)
            {
                item.Text = "";
            }
        }
       private void button14_Click(object sender, EventArgs e)
        {

            BitPC = Bit64 ? "64Bit" : "32Bit";
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            textBox12.Text = 
                   ("User Name:  " + UserName + "\r\n" + "\r\n" +
                   "PC Name:  " + MachineName + "\r\n" + "\r\n" +
                   "CPU:  " + CpuName + "\r\n" + "\r\n" +
                   "Threads: " + Threads + " Cores" + "\r\n" + "\r\n" +
                   "GPU:  " + GpuName + "\r\n" + "\r\n" +
                   "RAM Total and Max Increase: " + "\r\n" + "Total RAM: "+ TotalRam + "Gb" + "\r\n" +"Max RAM: " + MaxRam + "Gb"+"\r\n" + "\r\n" + "RAM Slots and Planks: " + RamName + "\r\n" + "\r\n" +
                   "64/32 Bit:  " + BitPC + "\r\n" + "\r\n" +
                   "OS Windows:  " + WindowsName + "\r\n" + "\r\n" +
                   "OS Build:  " + windowsVersion + "\r\n" + "\r\n" +
                   "Users: " + Users + "\r\n" + "\r\n" +
                   "System Path Directory:  " + Directory + "\r\n" + "\r\n" +
                   "System Up Time:  " + (int)perSystemCounter.NextValue() / 60 / 60 + " Hour" + "\r\n" + "\r\n" +
                   "RAM Available :  " + (int)MemoryCounter.NextValue() + " MB"+ "\r\n" + "\r\n" +
                   "Your IP address: " + address.ToString() + "\r\n"  );
        }


        private void button9_Click(object sender, EventArgs e)
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
                textBox11.Text = address.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
             label6.Text = "" + CpuName;
             label7.Text = "" + RamNameTwo;
             label5.Text = TotalRam + "Gb";

            float fcpu = CPU.NextValue();
            circularProgressBar1.Value = (int)fcpu;
            circularProgressBar1.Text  = string.Format("{0:0,00}%", fcpu);
            
             float dram = RAM.NextValue();
            circularProgressBar2.Value = (int)RAM.NextValue();
            circularProgressBar2.Text  = string.Format("{0:0,00}%", dram);
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)                     // move. mouse down, move, up
        {
            FormMove = true;
            StartPoint = new Point(e.X, e.Y);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
             if (FormMove)
               {
              Point p = PointToScreen(e.Location);
              Location = new Point(p.X - this.StartPoint.X, p.Y - this.StartPoint.Y);
             }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            textBox17.Text = "" + windowsVersion;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;           // svernut
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ToolTip toolTip1 = new ToolTip();                      // this is for notify. podskazki

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(pictureBox4, "Tap To Minimize");
            toolTip1.SetToolTip(pictureBox3, "Tap To Close");
            toolTip1.SetToolTip(textBox13, "Enter File Name");
            toolTip1.SetToolTip(button14, "Tap To Take All INFO");
            toolTip1.SetToolTip(button13, "Tap To Clear All Text Boxes"); 
            toolTip1.SetToolTip(button15, "Tap To Save Full Info Only");
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
             FormMove = false;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);          // save on desktop
            string path = filePath + @"\" + textBox13.Text + ".txt";

            if (textBox13.Text == "" || textBox13.Text == "ENTER FILE NAME HERE")
            {
                MessageBox.Show("Please, Enter File Name!", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!File.Exists(path))
                {
                    string LogInformation = textBox12.Text + Environment.NewLine;
                    File.WriteAllText(path, LogInformation);
                }
                MessageBox.Show("Log Saved in " + path);
            }
        }
        private void textBox13_Enter(object sender, EventArgs e)
        {
            if (textBox13.Text == "ENTER FILE NAME HERE")
            {
                textBox13.Text = "";

                textBox13.ForeColor = Color.Black;
            }
        }
        private void textBox13_Leave(object sender, EventArgs e)
        {
            if (textBox13.Text == "")
            {
                textBox13.Text = "ENTER FILE NAME HERE";

                textBox13.ForeColor = Color.Silver;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox15.Text = Directory;
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = UserName;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            textBox14.Text = "" + GpuName;
        }
    }
}
