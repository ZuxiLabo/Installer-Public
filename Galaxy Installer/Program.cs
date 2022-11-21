
//using HVL.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Windows.Forms;
using Tiny;

namespace HVL
{
    internal class Program
    {
        public static byte[] EACBYPASS = null;
        public static string CommandLine = null;
        public static bool IsDebug = false;
        public static int LogTracker = 0;
        private static string LogName = "Installer.log";
        public static int VersionID = 67;


        static void Main(string[] args)
        {

#if DEBUG
            IsDebug = true;
#endif
            CreateLog();

            Print("[Debug] Created LogFile", IsDebug);
#if !DEBUG
            //            Thread tAntiReverse = new Thread(new ThreadStart(AntiReverse));
            //          tAntiReverse.Start();
            Print("Started Anti Debug", IsDebug);
#endif
            if (!IsElevated)
            { Print("Warning Will Not Auto Start Injecter If you would like to Start Injecter Automatically Please Run As Admin"); Console.WriteLine(); Wait("Press Any Key To Accept This Warning"); Console.Write("\n"); }
            CommandLine = Utils.GetCommandLine().ToLower();
            if (CommandLine.Contains("--debug"))
            { IsDebug = true; Print("[Debug] Debug Mode Enabled", IsDebug); }
            Print($"[Debug] Command Line Args {CommandLine}", IsDebug);
            Print("[Debug] Done Pre Init Done Starting Installer Manager", IsDebug);
            CallAfterInit();
            Print("[Debug] Install Manager destroyed Success.", IsDebug);
            Print("[Debug] Exiting", IsDebug);
        }

        static void CallAfterInit()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                DoTOS();

                Print("Initilizing");

                // Print($"[Debug] INIT Installer Manager", IsDebug);
                Console.Title = "Galaxy Installer X By Cypher";
                Print("[Debug] Set Console Title Sucess ", IsDebug);
                #region Build Info
                /*Just Output Build Info*/
                string version = "X" + " Build ID: " + VersionID;
                string strCompTime = Properties.Resources.BuildTime;//.Replace("\n", "");
                Console.WriteLine();
                Print($"Installer Version {version}");
                Print($"Build Time {strCompTime}");
                Print("[Debug] Printed Info", IsDebug);
                Print("[Debug] Starting Auth", IsDebug);
                #endregion
                /*Init and Start Auth*/
                Print("Authenticating");
                
                Print("[Debug] Done Authing Getting User Info", IsDebug);




                // var Responce = Json.Decode<Dictionary<string, string>>(ServerResp.ToString());
                string Username = "FREE";

                try
                {

                  //  Responce.TryGetValue("Username", out Username);

                }
                catch (Exception ex)
                {

                    Print($"[Exception Handler] Caught An Exception Value Was Null.");
                    MessageBox.Show("HV20", "Internal Error");
                    Program.Print($"[Debug] {ex}", Program.IsDebug);
                    return;
                }

                Print("Authenticated");

                Print("[Debug] Done Checking If User Is Staff ", IsDebug);
                Console.WriteLine();
                Print("Installer Manager Initialised! =)");
                Console.WriteLine();
                Print("All These Stars And I Only Want You =(");
                Console.WriteLine();
                Print($"Welcome {Username}");
                Console.Title = $"Galaxy Installer X By HyperV | Logged In As {Username} ";

#if !DEBUG
                Console.Title = $"Galaxy Installer X By HyperV | Logged In As {Username} | DEBUG MODE";
#endif
                GetUserInput();
                //switch

            }
            catch (Exception e)
            {
                Print($"[Exception Handler] Caught An Exception");
                MessageBox.Show("HV20", "Internal Error");
                Program.Print($"[Debug] {e}", Program.IsDebug);
            }
        }

        static void GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n");
            Print("What Would You Like To Do Today");
            Print("1 Install / Repair Install");
            Print("2 Uninstall");
            Print("3 Run Injector");
            Print("4 Exit");
            var UserInputRaw = Console.ReadLine();
            Print($"[Debug] User Input {UserInputRaw}", IsDebug);
            int UserInput = int.Parse(UserInputRaw);
            Print($"[Debug] User Input Parsed {UserInput} ", IsDebug);

            switch (UserInput)
            {
                case 1:
                    Install();
                    break;
                case 2:
                    Uninstall();
                    break;
                case 3:
                    Print("Starting Injecter");
                    INITBypass.RunInject();
                    break;
                case 4:
                    Print($"[Debug] Destroying Inataller Manager & Exiting", IsDebug);

                    break;
                default:
                    GetUserInput();
                    break;
            }

        }

        static void Uninstall()
        {

            Print("\n\nWhat do you Want To Uninstall?");
            Console.ForegroundColor = ConsoleColor.Red;
            Print("1 Uninstall Everything \n2 Uninstall Just VRC Files\n3 Return To Previous Menu");
            var UserInputRaw = Console.ReadLine();
            Print($"[Debug] Uninstaller {UserInputRaw}", IsDebug);
            int UserInput = int.Parse(UserInputRaw);
            Print($"[Debug] Uninstaller {UserInput}", IsDebug);

            switch (UserInput)
            {
                case 1:

                    Console.ForegroundColor = ConsoleColor.Red;
                    Print("Are you Sure This Will Delete EVERYTHING includeing All Files Related To Any of HyperVoidLabs Projects \nIt will also Delete your AuthKey So YouWill Have to Enter it Again\n Please Type Yes if you want to Continue");
                    string a = Console.ReadLine();

                    if (a.ToLower() == "yes")
                    {
                        Print($"[Debug] Starting Removing Files...", IsDebug);
                        System.IO.DirectoryInfo di2 = new DirectoryInfo(Utils.GetAppDir(false, true));

                        foreach (FileInfo file in di2.GetFiles())
                        {
                            Print($"Deleted {file}");
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di2.GetDirectories())
                        {
                            dir.Delete(true);
                            Print($"Deleted {dir}");
                        }
                        File.Delete($"{Utils.GetVRChatPath()}\\VRCLoader.dll");
                        if (Directory.Exists($"{Utils.GetVRChatPath()}\\Modules"))
                        {

                            DirectoryInfo di = new DirectoryInfo($"{Utils.GetVRChatPath()}\\Modules");

                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                                Print($"Deleted {file}");
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                                Print($"Deleted {dir}");
                            }
                            Directory.Delete($"{Utils.GetVRChatPath()}\\Modules");
                        }
                        Print($"[Debug] Done Destroying Installer Manager", IsDebug);
                        Print("Deleted Everthing GoodBye");

                        Wait("Press Any Key To Exit ");

                    }
                    else { Uninstall(); }

                    break;
                case 2:
                    Print("Are you Sure this will Uninstall the Client? \nPlease Type Yes to Delete");
                    string b = Console.ReadLine();

                    if (b.ToLower() == "yes")
                    {
                        Print($"[Debug] Starting Deleting Client Only", IsDebug);

                        File.Delete($"{Utils.GetVRChatPath()}\\VRCLoader.dll");
                        if (Directory.Exists($"{Utils.GetVRChatPath()}\\Modules"))
                        {

                            DirectoryInfo di = new DirectoryInfo($"{Utils.GetVRChatPath()}\\Modules");

                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                                Print($"Deleted {file}");
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                                Print($"Deleted {dir}");
                            }
                            Directory.Delete($"{Utils.GetVRChatPath()}\\Modules");
                        }
                        Print($"[Debug] Done", IsDebug);

                        Print($"[Debug] Installer Manager Destroyed Waiting For User To Exit", IsDebug);
                        Wait("Done Press Enter To Exit");
                        return;
                    }
                    else { Uninstall(); }

                    Wait();
                    break;
                case 3:
                    GetUserInput();
                    break;
                default:
                    GetUserInput();
                    break;
            }

        }

        public static void Install()
        {
            try
            {
                Print($"[Debug] Stating Install Manager", IsDebug);
                Print("Installing\n");
                Print($"[Debug] Done Initalizing Staring Installer", IsDebug);
                EACBYPASS = null;

                INITBypass.InitBypass();
                InitVRCFiles.InitVRCFilesMeth();



                Print("Done Installing");

#if !DEBUG

                //   Process.Start(Utils.GetVRChatPath() + "\\start_protected_game.exe");
                //INITBypass.RunInject();
                Print("Start VRChat You May Need To Reinstall After VRChat Updates...");
                Print($"[Debug] Started VRChat", IsDebug);

#endif
                Print($"[Debug] Done Installing Destroying Installer Manager", IsDebug);

                Wait("Done Press Any Key to Close");
            }
            catch (Exception e)
            {
                Console.Write(e);
                Console.ReadKey();
            }


        }

        static bool IsElevated
        {
            get
            {
                var id = WindowsIdentity.GetCurrent();
                return id.Owner != id.User;
            }
        }

        static void Wait(string A = null) { if (A != null) { Print(A); } Console.ReadKey(); }

        public static void CreateLog()
        {


            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + LogName))
            { File.Delete(Directory.GetCurrentDirectory() + "\\" + LogName); }
            File.Create(Directory.GetCurrentDirectory() + "\\" + LogName).Close();

        }

        public static void Print(object toWrite = null, bool ShouldWrite = true)
        {
            if (ShouldWrite)
            { Console.WriteLine(toWrite); }




            using (StreamWriter sw = File.AppendText(LogName))
            {
                sw.WriteLine(LogTracker + " " + " ~> " + toWrite);
            }
            LogTracker++;
        }

        internal static void DoTOS()
        {

            var wc = new WebClient();
            wc.Headers["User-Agent"] = "GalaxyKEKDLFILE";
            Print("[Debug] Showing TOS", IsDebug);
            // Process.Start("https://hvl.gg/tos");

            if (!IsDebug)
                Console.Clear();

            Console.Title = "Please Accept This TOS To Continue ";

            string tos = wc.DownloadString("https://hvl.gg/ClientStuff/rawtos.txt");

            Console.WriteLine(tos);
            Print("[Debug] Shown TOS", IsDebug);
            Console.WriteLine();
            Wait("Press Any Key To Accept This TOS");
            if (!IsDebug)
                Console.Clear();
            Print("[Debug] Accepted TOS", IsDebug);

            wc.Dispose();

        }


    } 



}


