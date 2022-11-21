using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HVL
{
    internal class INITBypass
    {
        public static void InitBypass()
        {
            var wc = new WebClient();
            wc.Headers["User-Agent"] = "GalaxyKEKDLFILE";




            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/InjectorV23/InjectV3", $"{Utils.GetAppDir(true)}\\Bypass.exe");
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/InjectorV23/VRCInit", $"{Utils.GetAppDir(true)}\\VRCInit.dll");
          
           

            /*

            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/EACBypass.exe", $"{Utils.GetAppDir(true)}\\Bypass.exe");
            Program.Print("[Debug] Downloaded EACBypass", Program.IsDebug);
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/InjectDLL.dll", $"{Utils.GetAppDir(true)}\\test.dll"); Program.Print("[Debug] Downloaded Injected DLL", Program.IsDebug);
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/Aga.Controls.dll", $"{Utils.GetAppDir(true)}\\Aga.Controls.dll"); Program.Print("[Debug] Downloaded Controller", Program.IsDebug);
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/auto.js", $"{Utils.GetAppDir(true)}\\auto.js"); Program.Print("[Debug] Downloaded auto.js", Program.IsDebug);
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/assembler.lnk", $"{Utils.GetAppDir(true)}\\assembler.lnk"); Program.Print($"[Debug] Downloaded Linker File", Program.IsDebug);
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/_spoofer_stub.asm", $"{Utils.GetAppDir(true)}\\_spoofer_stub.asm"); Program.Print("[Debug] Downloaded Stub", Program.IsDebug);
            wc.DownloadFile("https://cdn.hyperv.one/favicon.ico", $"{Utils.GetAppDir(true)}\\favicon.ico"); Program.Print($"[Debug] Downloaded Icon", Program.IsDebug);
            //   wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Injector/Shortcut", $"{Directory.GetCurrentDirectory()}\\Galaxy Bypass.lnk");*/
            // CreateShortcut();
        }

        public static void RunInject()
        {
            if (System.IO.File.Exists($"{Utils.GetAppDir(true)}\\Bypass.exe"))
            {

                var startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = Utils.GetAppDir(true);
                    startInfo.FileName = $"{Utils.GetAppDir(true)}\\Bypass.exe";
                Process.Start(startInfo);
                Program.Print($"[Debug] Started Injecter Success", Program.IsDebug);
            }
            else { Program.Print("Injector Not Found Installing"); Program.Install(); }

        }
        public static void CreateShortcut()
        {
            WshShell shell = new WshShell();
            string shortcutAddress = Directory.GetCurrentDirectory() + @"\Galaxy Bypass.lnk"; ;//(string)shell.SpecialFolders.Item(ref shDesktop) + @"\Notepad.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "FUCK EAC";
            // shortcut.Hotkey = "Ctrl+Shift+N";
            shortcut.TargetPath = Utils.GetAppDir(true) + "\\Bypass.exe";//Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\notepad.exe";
            shortcut.IconLocation = Utils.GetAppDir(true) + "\\favicon.ico";
            shortcut.Save();
        }
   }
}
