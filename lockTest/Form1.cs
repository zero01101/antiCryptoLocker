using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace lockTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
            File.Copy("ht.dat", path + "\\ht.exe", true);
            Process p = new Process();            
            p.StartInfo.FileName = path + "\\ht.exe";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            int result = 0;
            try
            {
                p.Start();
                p.WaitForExit();
                result = p.ExitCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("lockdown seems successful", ":)");
                File.Delete(path + "\\ht.exe");
                p.Dispose();
                Application.Exit();
            }
            File.Delete(path + "\\ht.exe");
            p.Dispose();
            if (result != 0)
            {
                MessageBox.Show("lockdown seems successful", ":)");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("lockdown seems unsuccessful - did you reboot after lockdown?", ":(");
                Application.Exit();
            }            
        }
    }
}
