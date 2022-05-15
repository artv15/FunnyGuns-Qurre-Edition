using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CommandSystem;
using System.ComponentModel;
using MEC;

namespace FunnyGunsRecoded.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class forceupdate : ICommand
    {
        public string Command => "fg_forceupdate";

        public string[] Aliases => null;

        public string Description => "Forcibly update plugin to the latest version (if autoupdate breaks for somewhat reason)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.debugUpdateWarning && Plugin.IsDebugEnabled)
            {
                response = "Hey! That seems that you are running a debug build! That's pretty cool! If you will force an update right now, then release edition will be installed on top of debug edition! Use this command again in 20 seconds to force an update!";
                Plugin.debugUpdateWarning = true;
                Timing.CallDelayed(20f, () => Plugin.debugUpdateWarning = false);
                return false;
            }
            else
            {
                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += wc_DownloadComplete;
                        wc.DownloadFileAsync(new Uri("https://treesholdapi.ml/FunnyGuns/FunnyGunsRecoded.dll"), Qurre.PluginManager.PluginsDirectory + "/FunnyGunsRecoded.dll");
                    }
                    catch (Exception ex)
                    {
                        Qurre.Log.Warn($"Update failed! Error: {ex.Message}");
                    }
                }
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
    }
}
