﻿using System;
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

        public string Description => "Forcibly update plugin to the latest version";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
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

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Qurre.Log.Info("Downloading an update. Progress: " + e.ProgressPercentage.ToString());
        }

        void wc_DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            Qurre.Log.Info("Successfully updated plugin to current version! Server restart will commence in T-10 seconds!");
            Timing.CallDelayed(10f, () => Qurre.API.Server.Restart());
        }
    }
}