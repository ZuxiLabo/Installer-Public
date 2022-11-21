using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HVL
{
    internal class InitVRCFiles
    {
        public static void InitVRCFilesMeth()
        {
            var wc = new WebClient();
            wc.Headers["User-Agent"] = "GalaxyKEKDLFILE";

            if (!Directory.Exists($"{Utils.GetVRChatPath()}\\Modules"))
            {
                Directory.CreateDirectory($"{Utils.GetVRChatPath()}\\Modules");
            }
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Galaxy/VRCLoader.d", $"{Utils.GetVRChatPath()}\\VRCLoader.dll");
            Program.Print("[Debug] Downloaded VRCLoader.dll", Program.IsDebug);
            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Galaxy/GalaxyLoader.d", $"{Utils.GetVRChatPath()}\\Modules\\GalaxyLoader.dll");
            Program.Print("[Debug] Downloaded GalaxyLoader.dll", Program.IsDebug);

            if (!File.Exists($"{Utils.GetVRChatPath()}\\Start.exe")) 
            System.IO.File.Move($"{Utils.GetVRChatPath()}\\start_protected_game.exe", $"{Utils.GetVRChatPath()}\\Start.exe");

            wc.DownloadFile("https://dl.galaxyvrc.xyz/Cheats/Galaxy/VRCInit.exe", $"{Utils.GetVRChatPath()}\\start_protected_game.exe");


        }
        
        public static void DeleteVRCFiles()
        {
            if (Directory.Exists($"{Utils.GetVRChatPath()}\\Modules"))
            {
                
                File.Delete($"{Utils.GetVRChatPath()}\\VRCLoader.dll");
            }
            
           

        }
    }
}
