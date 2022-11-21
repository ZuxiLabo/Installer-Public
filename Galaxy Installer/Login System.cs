using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tiny;

namespace HVL
{
    internal class Server
    {
        protected internal static object SendPostRequestInternal(string EndPoint, Dictionary<string, string> SendData = null, int ParseOnReceve = 0)
        {


            return null; 
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    #region Setup Request Headers & Add Required Values
                    httpClient.Timeout = TimeSpan.FromMinutes(1);
                    if (SendData == null)
                        SendData = new Dictionary<string, string>();

                    SendData.Add("Key", Utils.GetKey());
                    
                   /*INFO OMITTED TO KEEP FROM LEAKING EVEN THOUGH IT WILL BE CHANGED LATER LOL*/

                    
                    #endregion
                    #region Send Post Request Get Responce Values
                    string PostURI = SendServerConfig.APIBaseEndpoint + EndPoint;
                    Task<HttpResponseMessage> async = httpClient.PostAsync(PostURI, new FormUrlEncodedContent(SendData));
                    async.Wait();
                    HttpResponseMessage responce = async.Result;
                    HttpStatusCode statusCode = responce.StatusCode;
                    #endregion
                    #region If Request is Ok Should We Parse it and How
                    if (statusCode == HttpStatusCode.OK)
                    {

                        httpClient.Dispose();
                        switch (ParseOnReceve)
                        {
                            case 1:
                                Task<string> ParseThisValue = responce.Content.ReadAsStringAsync();
                                var Responce = Json.Decode<Dictionary<string, int>>(ParseThisValue.ToString());
                                return Responce;
                            case 2:
                                Task<string> StringSetup = responce.Content.ReadAsStringAsync();
                                return StringSetup.Result;
                            case 3:

                                return true;
                            default:
                                return responce.Content.ReadAsStreamAsync();
                        }
                    }
                    #endregion
                    #region If It Is a Bad request Tell The User with the Server Responce
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Task<string> DES = responce.Content.ReadAsStringAsync();


                        if (DES.Result.Contains("<"))
                        {
                            MessageBox.Show("HV15", "HV15");
                            return null;
                        }

                        var Responce = Json.Decode<Dictionary<string, string>>(DES.Result);
                        Responce.TryGetValue("message", out var message);
                        Program.Print($"Failed to Send Request to {EndPoint} Server responded with (Error: {message})");

                        if (message.Contains("HV04"))
                        {
                            File.WriteAllText(Utils.GetAppDir(false, true) + "\\HyperVoid.Auth", "");
                        }

                        if (message.Contains("HV"))
                        {
                           MessageBox.Show(message, EndPoint + " Error " + message);
                            return null;
                        }

                        httpClient.Dispose();
                        Program.Print($"[Debug] WebClient Disposed", Program.IsDebug);
                        Program.Print($"[Debug] Distroying Installer Manager", Program.IsDebug);
                       // Program.Print("Press any Key To EXIT");
                      //  Console.ReadKey();
                    }
                    httpClient.Dispose();
                    return null;
                    #endregion
                }
            }
            catch (Exception e)
            {
                Program.Print($"[Exception Handler] Caught An Exception while trying to {EndPoint} Report To Hyper");
                MessageBox.Show("HV20", "Load Error");
                Program.Print($"[Debug] [PRI] [{EndPoint}] {e}", Program.IsDebug);
                return null;
            }
        }
        protected internal class SendServerConfig
        {
            protected internal static string APIBaseEndpoint = "https://api.hvl.gg/";
            protected internal readonly static string Version = "V" + Program.VersionID;
            protected internal readonly static string UA = "Galaxy_Installer" + Version;
            protected internal readonly static string CA = "c644c984f19d6af6bb3808726997cc749828f2f800a9fb5d16f57bbe75b1725c" + Version;
        }

    }
    



}

