using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace antiCryptoLocker
{
    class regops
    {        
        string policy = @"SOFTWARE\Policies\Microsoft\Windows\safer\codeidentifiers"; //for getting/deleting subkeys with registry.localmachine 
        string fullpolicy = @"SOFTWARE\Policies\Microsoft\Windows\safer\codeidentifiers\0\Paths"; //same but includes expected keys        

        public string checkForLockdown()
        {
            //checks if we've got any path values first - assume locked if we do
            string result = "";
            RegistryKey key0;
            RegistryKey keyPaths;
            try
            {
                key0 = Registry.LocalMachine.OpenSubKey(policy + @"\0");
                if (key0 == null)
                {
                    Registry.LocalMachine.CreateSubKey(policy + @"\0");
                }
            }
            catch (Exception ex)
            {
                Registry.LocalMachine.CreateSubKey(policy + @"\0");
            }
            try
            {
                keyPaths = Registry.LocalMachine.OpenSubKey(policy + @"\0\Paths");
                if (keyPaths.SubKeyCount == 0)
                {
                    result = "lockdown";
                }
                else if (keyPaths.SubKeyCount > 0)
                {
                    int score = 0;
                    foreach (string dir in keyPaths.GetSubKeyNames())
                    {
                        try
                        {
                            object sanity = Registry.GetValue(keyPaths + "\\" + dir,"Description", "nothing to see here");
                            if (sanity.ToString() == "antiCryptoLocker")
                            {
                                score++;
                            }
                        }
                        catch (Exception ex)
                        {
                            //WAT DO?!?
                        }
                    }
                    if (score > 0)
                    {
                        result = "unlock";
                    }
                    else
                    {
                        result = "lockdown";
                    }
                }
                else if (keyPaths == null)
                {
                    Registry.LocalMachine.CreateSubKey(policy + @"\0\Paths");
                    result = "lockdown";
                }                
            }
            catch (Exception ex)
            {
                Registry.LocalMachine.CreateSubKey(policy + @"\0\Paths");
                result = "lockdown";
            }
            return result;
        }

        public void ensureDefaults()
        {
            //check for values in codeidentifiers first - authenticodeenabled, DefaultLevel, PolicyScope, TransparentEnabled
            RegistryKey basekey = Registry.LocalMachine.OpenSubKey(policy);
            try
            {
                string authenticodeenabled = basekey.GetValue("authenticodeenabled").ToString();
            }
            catch (Exception ex)
            {
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + policy, "authenticodeenabled", 0, RegistryValueKind.DWord);                
            }
            try
            {
                string DefaultLevel = basekey.GetValue("DefaultLevel").ToString();
            }
            catch (Exception ex)
            {
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + policy, "DefaultLevel", 262144, RegistryValueKind.DWord);                 
            }
            try
            {
                string PolicyScope = basekey.GetValue("PolicyScope").ToString();
            }
            catch (Exception ex)
            {
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + policy, "PolicyScope", 0, RegistryValueKind.DWord);                
            }
            try
            {
                string TransparentEnabled = basekey.GetValue("TransparentEnabled").ToString();
            }
            catch (Exception ex)
            {
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + policy, "TransparentEnabled", 1, RegistryValueKind.DWord);                            
            }          
        }

        public int lockdown(string pathtolock, string guid)
        {
            guid = "{" + guid + "}";
            int count = 0;
            try
            {                
                Registry.LocalMachine.CreateSubKey(fullpolicy + "\\" + guid);
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + fullpolicy + "\\" + guid, "Description", "antiCryptoLocker", RegistryValueKind.String);
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + fullpolicy + "\\" + guid, "ItemData", pathtolock, RegistryValueKind.ExpandString);
                Registry.SetValue(Registry.LocalMachine.ToString() + "\\" + fullpolicy + "\\" + guid, "SaferFlags", 0, RegistryValueKind.DWord);
                count++;
            }
            catch (Exception ex)
            {
                //WAT DO?!?
            }
            return count;            
        }

        public int unlock()
        {
            int count = 0;
            RegistryKey lockedpaths = Registry.LocalMachine.OpenSubKey(fullpolicy);
            foreach (string s in lockedpaths.GetSubKeyNames())
            {
                try
                {
                    object sanity = Registry.LocalMachine.OpenSubKey(fullpolicy + "\\" + s).GetValue("Description");

                    if (sanity.ToString() == "antiCryptoLocker")
                    {
                        //Registry.LocalMachine.DeleteSubKeyTree(fullpolicy + "\\" + s, true);
                        Registry.LocalMachine.DeleteSubKeyTree(fullpolicy + "\\" + s);
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    //WAT DO?!?
                }
            }
            if (count > 0)
            {
                gpupdate();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no changes were made - are you sure you're running as admin?\r\ndo those reg keys have weird permissions?");
            }
            return count;
        }

        public void gpupdate()
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "gpupdate.exe";
                p.StartInfo.Arguments = " /force";
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();
                p.Dispose();
            }
            catch (Exception ex)
            {
                //probably don't have gpupdate.exe - xp home :/
            }
            System.Windows.Forms.MessageBox.Show("you'll probably need to restart your computer now.");
        }
    }
    
    
}
