using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVL
{
    internal class Utils
    {
        public static string SteamPath
        {
            get
            {
                return Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "SteamPath", null).ToString();
            }
        }

        internal static string GetCommandLine()
        {
            return Environment.CommandLine.ToLower();
        }

        protected internal static string GetVRChatPath()
        {
            string path = Utils.SteamPath + "/steamapps/libraryfolders.vdf";
            if (File.Exists(path))
            {
                try
                {
                    string[] array = File.ReadAllLines(path);
                    List<string> list = new List<string>();
                    foreach (string text in array)
                    {
                        if (text.Contains("path"))
                        {
                            list.Add(text);
                        }
                    }
                    foreach (string text2 in list)
                    {
                        string text3 = (text2 + "\\\\steamapps\\\\common\\\\VRChat").Substring(text2.IndexOf(":\\") - 3).Replace("\"", "").Replace("\\\\", "\\").Trim();
                        if (Directory.Exists(text3))
                        {
                            return text3;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Print("[ERROR]: " + ex.Message);
                }
            }
            return null;
        }


        internal static string GetKey()
        {
            if (!File.Exists(GetAppDir(false, true) + "\\HyperVoid.Auth"))
            {
                File.Create(GetAppDir(false, true) + "\\HyperVoid.Auth").Close();
            }
            if (new FileInfo(GetAppDir(false, true) + "\\HyperVoid.Auth").Length <= 0)
            {
                Program.Print("Waiting For Auth Promt.", Program.IsDebug);
                //  var input = Console.ReadLine();
                var input = Interaction.InputBox("Please Enter Your AuthKey", "Galaxy Installer X", "");

                File.WriteAllText(GetAppDir(false, true) + "\\HyperVoid.Auth", input.Trim());
            }
            return File.ReadAllText(GetAppDir(false, true) + "\\HyperVoid.Auth").Trim();
        }
        public static string GetHWID()
        {
            string HWID = "";

            string name = "SOFTWARE\\Microsoft\\Cryptography";
            string name2 = "MachineGuid";
            using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
                {
                    if (registryKey2 != null)
                    {
                        object value = registryKey2.GetValue(name2);
                        if (value != null)
                            HWID = value.ToString();

                    }
                }
                return HWID;
            }
        }
        

        public static string GetAppDir(bool hidden = false, bool Root = false)
        {
            string Dir = "";

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string AppFolder = Path.Combine(folder, "HyperVoid Labs");


            if (!Directory.Exists($"{AppFolder}\\GalaxyInjecterA"))
            {
                Directory.CreateDirectory($"{AppFolder}");
                Directory.CreateDirectory($"{AppFolder}\\GalaxyInjecter");
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\GalaxyInjecter");
                DirectoryInfo di = Directory.CreateDirectory($"{AppFolder}\\GalaxyInjecter\\$internal");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            }


            if (hidden)
            { Dir = Path.Combine(folder, "HyperVoid Labs\\GalaxyInjecter\\$internal"); }
            else
            {
                if (Root)
                { Dir = Path.Combine(folder, "HyperVoid Labs"); }
                else
                { Dir = Path.Combine(folder, "HyperVoid Labs\\GalaxyInjecter"); }
            }

            return Dir;
        }
    }
}
