using CommandSystem;
using MEC;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;

namespace FunnyGunsRecoded.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class forceupdate : ICommand
    {
        public string Command => "fg_forceupdate";

        public string[] Aliases => null;

        public string Description => "Forcibly update plugin to the latest RELEASE version (if autoupdate breaks for somewhat reason)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.debugUpdateWarning && Plugin.IsDebugEnabled)
            {
                response = "Hey! This is releae edition update command. Executing it now will install release edition and delete debug edition. If you want to update to newest debug build, use fg_debugupdate. If you want to switch to release branch, use fg_forceupdate again!";
                Plugin.debugUpdateWarning = true;
                Timing.CallDelayed(20f, () => Plugin.debugUpdateWarning = false);
                return false;
            }
            else
            {
                Task.Run(() => reqAsyncRelease());
                response = "Forcing an update!";
                return true;
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Qurre.Log.Info("Downloading an update. Progress: " + e.ProgressPercentage.ToString());
        }

        void wc_DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            Qurre.Log.Info("Successfully updated plugin to current version! Server restart will commence in T-5 seconds!");
            Timing.CallDelayed(5f, () => Qurre.API.Server.Restart());
        }

        async Task<bool> reqAsyncRelease()
        {
            using (HttpClient hc = new HttpClient())
            {
                var result = await hc.GetAsync("https://treesholdapi.ml/FunnyGuns/download.php");
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Qurre.Log.Error("Update to release failed! Unauthorized!");
                    return false;
                }
                else if (result.StatusCode == HttpStatusCode.OK)
                {
                    Qurre.Log.Info("OK! Trying to download...");
                    Qurre.Log.Info("Downloading an update.");
                    using (var fs = new FileStream(
                        Qurre.PluginManager.PluginsDirectory + "/FunnyGunsRecoded.dll",
                        FileMode.Create))
                    { 
                        await result.Content.CopyToAsync(fs);
                    }
                    Qurre.Log.Info("Successfully updated plugin to current release version! Server restart will commence in T-5 seconds!");
                    Timing.CallDelayed(5f, () => Qurre.API.Server.Restart());
                    return true;
                }
                else
                {
                    Qurre.Log.Error($"Update to newest release failed! Remote error. Errorcode: {(int)result.StatusCode}");
                    return false;
                }
            }
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class forceupdateDebug : ICommand
    {
        public string Command => "fg_debugupdate";

        public string[] Aliases => null;

        public string Description => "Forcibly update plugin to the latest DEBUG version (Requires APIKEY)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
             Task.Run(() => reqAsyncDebug());
             response = "Forcing an update!";
             return true;
        }

        

        async Task<bool> reqAsyncDebug()
        {
            using (HttpClient hc = new HttpClient())
            {
                var auth = new authDebugUpdate(); // Generating new class for post requests
                auth.APIKey = Plugin.CustomConfig.APIKey; // Assigning an APIKey from config file
                string post = "{\"APIKey\": \"" + auth.APIKey + "\"}"; // Posting APIKey to remote
                var content = new StringContent(post, Encoding.UTF8, "application/json"); // Generating content
                var result = await hc.PostAsync("https://treesholdapi.ml/FunnyGunsDevelopment/download.php", content); // Sending HTTP POST request.
                if (result.StatusCode == HttpStatusCode.Unauthorized) // Invalid apikey
                {
                    Qurre.Log.Error("Update to debug failed! Invalid APIKey! If you believe that this is a mistake and the key is correct, contact Treeshold#0001. Maybe the key is expired.");
                    return false; // returning false = not cool
                }
                else if (result.StatusCode == HttpStatusCode.OK) // Key is valid, go on
                {
                    Qurre.Log.Info("OK! Trying to download...");
                    Qurre.Log.Info("Downloading an update.");
                    using (var fs = new FileStream( // Creating File stream to replace file in %appdata%/Qurre/Plugins
                        Qurre.PluginManager.PluginsDirectory + "/FunnyGunsRecoded.dll", // This file
                        FileMode.Create)) // In create mode
                    {
                        await result.Content.CopyToAsync(fs); // Awaiting until we successfuilly replace this file with a new one (or create new one)
                    }
                    Qurre.Log.Info("Successfully updated plugin to current debug version! Restart the server to apply changes (full round restart will suffice)"); // Responding that we did it, yay!
                    //Timing.CallDelayed(5f, () => Qurre.API.Server.Restart());
                    return true; // returning true = cool
                }
                else // Other status code, maybe we fucked up
                {
                    Qurre.Log.Error($"Update to newest debug failed! Remote error. Errorcode: {(int)result.StatusCode}");
                    return false; // returning false = not cool
                }
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Qurre.Log.Info("Downloading an update. Progress: " + e.ProgressPercentage.ToString());
        }

        void wc_DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            Qurre.Log.Info("Successfully updated plugin to current version!  Restart the server to apply changes (full round restart will suffice)");
            //Timing.CallDelayed(5f, () => Qurre.API.Server.Restart());
        }
    }

    class authDebugUpdate
    {
        public string APIKey;
    }
}
