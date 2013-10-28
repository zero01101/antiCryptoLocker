using System;
using System.Windows.Forms;
using System.Security.Principal;
using System.IO;
using System.Diagnostics;

namespace antiCryptoLocker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }                
        
        regops reg = new regops();        
        Guid g;
        bool isadmin;

        private void Form1_Load(object sender, EventArgs e)
        {
            //check for admin privileges first            
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isadmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isadmin = false;
            }
            catch (Exception ex)
            {
                isadmin = false;
            }
            if (!isadmin)
            {
                MessageBox.Show("You must have administrative rights to use this application.", "ERROR");
                Application.Exit();
            }
            else 
            {
                string lockorunlock = reg.checkForLockdown();
                switch (lockorunlock)
                {
                    case "lockdown":
                        btnLockdown.Enabled = true;
                        break;
                    case "unlock":
                        btnUnlock.Enabled = true;
                        break;
                }
                reg.ensureDefaults();
            }
        }

        private void btnLockdown_Click(object sender, EventArgs e)
        {
            string[] directories = { @"%appdata%\*.exe", @"%localappdata%\*.exe", @"%userprofile%\AppData\Local\Temp\*.exe", 
                                     @"%userprofile%\AppData\Local\Temp\wz*\*.exe", @"%userprofile%\AppData\Local\Temp\7z*\*.exe", 
                                     @"%userprofile%\AppData\Local\Temp\*.zip\*.exe", @"%userprofile%\AppData\Local\Temp\rar*\*.exe", 
                                     @"%appdata%\*\*.exe", @"%localappdata%\*\*.exe" };
            int count = 0;
            foreach (string dir in directories)
            {
                g = Guid.NewGuid();
                count += reg.lockdown(dir, g.ToString());
            }
            if (count == 0)
            {
                System.Windows.Forms.MessageBox.Show("no changes were made - are you sure you're running as admin?\r\ndo those reg keys have weird permissions?");
            }
            else
            {
                reg.gpupdate();
                btnLockdown.Enabled = false;
                btnUnlock.Enabled = true;
            }
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            int deletes = reg.unlock();
            if (deletes > 0)
            {
                btnUnlock.Enabled = false;
                btnLockdown.Enabled = true;
            }
        }
    }
}
