using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Utf8Json;

namespace FunnyGunsRecoded
{
    public class Plugin : Qurre.Plugin
    {
        #region overrides
        public override string Developer { get; } = "Treeshold (aka Star Butterfly) | I hate letter g"; /* <- plugin developer(optional) */
        public override string Name { get; } = "Funny Guns"
#if DEBUG
            + " (Debug Edition)";
#else
            + " (Release Edition)";
#endif
        public override System.Version Version { get; } = new System.Version(0, 7, 4, 0); /* <- plugin version(optional) */
        public static Config CustomConfig { get; private set; } /* <- creating a new config class */

        public static bool debugUpdateWarning { get; set; } = false;
#if DEBUG
        public static bool IsDebugEnabled = true;
#else
        public static bool IsDebugEnabled = false;
#endif

        #endregion

        #region globals
        /// <summary>
        /// True = requires update from remote.
        /// </summary>
        public static bool isOutdated = true;

        /// <summary>
        /// Selected locale. Managed by Classes.LocalisationManager
        /// </summary>
        public static Interfaces.ILocalisation selectedLocale;

        /// <summary>
        /// Amount of deaths before Tutorial Assault. Used to calculate chance of Assault.
        /// </summary>
        public static int HowManyDeathSinceLastAssault = 0;
        
        /// <summary>
        /// Amount of Chaos Agents, assigned by HUD Coroutine
        /// </summary>
        public static int CI = 0;

        /// <summary>
        /// Some sort of hotfix, i guess...
        /// </summary>
        public static int AssaultWasStaredHowManyTimes = 0;

        /// <summary>
        /// Amount of MTF, assigned by HUD Coroutine
        /// </summary>
        public static int NTF = 0;

        /// <summary>
        /// Mutators, which are currently engaged (they are impacting the game rn)
        /// </summary>
        public static List<Classes.Mutator> engagedMutators = new List<Classes.Mutator>();

        /// <summary>
        /// Mutators, which are currently loaded (they are picked during the game (stage incrementation))
        /// </summary>
        public static List<Classes.Mutator> loadedMutators = new List<Classes.Mutator>();


        Events ev;

        /// <summary>
        /// True = event IS started, False = event IS NOT started.
        /// </summary>
        public static bool isEngaged = false;

        /// <summary>
        /// True = stage is frozen, False = It is not.
        /// </summary>
        public static bool isStageFrozen = false;

        /// <summary>
        /// If set to false, will not stop event on NEP
        /// </summary>
        public static bool checkForEndgame = true;

        /// <summary>
        /// Seconds, before stage will be incremented
        /// </summary>
        public static int SecondsBeforeNextStage = 0;

        /// <summary>
        /// Locale mutator names go here!
        /// </summary>
        public static Dictionary<string, string> MutatorLocaleDict = new Dictionary<string, string>();

        /*/// <summary>
        /// Used by damage++ mutator.
        /// </summary>
        public static bool damage3x = false;*/

        /// <summary>
        /// Stage.
        /// 0 = PreparationStage
        /// 1 = Stage 1 (No mutators)
        /// 2 = Stage 2 (One mutator)
        /// 3 = Stage 3 (Two mutators)
        /// 4 = Stage 4 (Three mutators)
        /// 5 = Instant Death (No mutators, 2 HP is drained every second)
        /// </summary>
        public static int Stage = 0;
        #endregion

        #region Enable/Disable
        public override void Enable()
        {
            CustomConfig = new Config();
            CustomConfigs.Add(CustomConfig);
            checkVersion();
            Classes.LocalisationManager.InitLocalisation(CustomConfig.Locale);
            ev = new Events();
            Qurre.Events.Player.Dies += ev.playerDied;
            Qurre.Events.Player.RoleChange += ev.roleChanging;
            Qurre.Events.Round.Waiting += ev.WaitingForPlayers;
            Qurre.Events.Player.DamageProcess += ev.hurting;
            Qurre.Events.Player.RechargeWeapon += ev.onReloading;
            Qurre.Events.Round.TeamRespawn += ev.TeamRespawn;
            Qurre.Events.Player.DropAmmo += ev.droppingAmmo;
            Qurre.Events.Player.ItemUsed += ev.ItemUsed;
        }

        public override void Disable()
        {
            CustomConfigs.Clear();
            MutatorLocaleDict.Clear();
            Qurre.Events.Player.Dies -= ev.playerDied;
            Qurre.Events.Player.RoleChange -= ev.roleChanging;
            Qurre.Events.Round.Waiting -= ev.WaitingForPlayers;
            Qurre.Events.Player.DamageProcess -= ev.hurting;
            Qurre.Events.Player.RechargeWeapon -= ev.onReloading;
            Qurre.Events.Round.TeamRespawn -= ev.TeamRespawn;
            Qurre.Events.Player.DropAmmo -= ev.droppingAmmo;
            Qurre.Events.Player.ItemUsed -= ev.ItemUsed;
        }
        #endregion

        #region updates
        public class version
        {
            public bool success;
            public int major;
            public int minor;
            public int patch;
            public int revision;
        }

        public static string getWarnColor()
        {
            var colorAlert = "red";
            switch (Plugin.SecondsBeforeNextStage % 2)
            {
                case 0:
                    colorAlert = "red";
                    break;
                case 1:
                    colorAlert = "white";
                    break;
            }
            return colorAlert;
        }

        public static string getStageColor()
        {
            string color = "#fc2d2d";
            switch (Plugin.Stage)
            {
                case 1:
                    color = "#42fc2d";
                    break;
                case 2:
                    color = "#bafc2d";
                    break;
                case 3:
                    color = "#fcbe2d";
                    break;
                case 4:
                    color = "#fc2d2d";
                    break;
            }
            return color;
        }

        public static string getDisplayByCommandName(string cmd_name)
        {
            try
            {
                if (Plugin.MutatorLocaleDict.ContainsKey(cmd_name))
                {
                    return Plugin.MutatorLocaleDict[cmd_name];
                }
                else
                {
                    Qurre.Log.Error($"Localisation for {cmd_name} not found!");
                    return $"<color=red>Localization NF for:</color> {cmd_name}";
                }
            }
            catch (Exception e)
            {
                Qurre.Log.Error($"An exception occured during mutator displayname defining. Error: {e.Message}");
                return $"<color=red>Localization errorred for:</color> {cmd_name}";
            }
        }

        async Task<bool> reqAsyncRelease()
        {
            using (HttpClient hc = new HttpClient())
            {
                var result = await hc.GetAsync("https://treesholdapi.ml/FunnyGuns/download.php");
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Qurre.Log.Error("Update to release failed! Unauthorized! (this is really unexpected!)");
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
                    Qurre.Log.Info("Successfully updated plugin to current release version! Restart the server to apply changes! Also, full round restart will suffice!");
                    return true;
                }
                else
                {
                    Qurre.Log.Error($"Update to newest release failed! Remote error. Errorcode: {(int)result.StatusCode}");
                    return false;
                }
            }
        }

        void checkVersion()
        {
            if (!Plugin.IsDebugEnabled && CustomConfig.Autoupdates)
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        string JSON_raw = wc.DownloadString("https://treesholdapi.ml/FunnyGuns/version.php");
                        version verRemote = JsonSerializer.Deserialize<version>(JSON_raw);
                        isOutdated = true;
                        Version remt = new Version(verRemote.major, verRemote.minor, verRemote.patch, verRemote.revision);
                        int local, remote;
                        local = this.Version.Major * 1000 + this.Version.Minor * 100 + this.Version.Build * 10 + this.Version.Revision;
                        remote = remt.Major * 1000 + remt.Minor * 100 + remt.Build * 10 + remt.Revision;
                        Qurre.Log.Custom($"Checked for versions, remote said he has {remote}, i think we have {local}.");
                        if (local < remote)
                        {
                            Qurre.Log.Custom($"Plugin is oudated! Your version: {this.Version.Major}.{this.Version.Minor}.{this.Version.Build}.{this.Version.Revision}, Remote has {remt.Major}.{remt.Minor}.{remt.Build}.{remt.Revision}. Trying to autoupdate!", "FunnyGuns updater", ConsoleColor.DarkRed);
                        }
                        else
                        {
                            isOutdated = false;
                            Qurre.Log.Custom($"Your copy of plugin is up to date!", "FunnyGuns updater", ConsoleColor.Green);
                        }
                        if (isOutdated)
                        {
                            Qurre.Log.Custom("Autoupdate started!", "FunnyGuns updater", ConsoleColor.DarkGreen);
                            try
                            {
                                reqAsyncRelease();
                            }
                            catch (Exception ex)
                            {
                                Qurre.Log.Warn($"<color=darkred>Autoupdate failed! Error: {ex.Message}. Try using fg_forceupdate</color>");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Qurre.Log.Warn($"<color=darkred>Failed to check for updates! There seems to be an issue with the remote! Error: {ex.Message}</color>");
                }
            }
            else if (Plugin.IsDebugEnabled)
            {
                Qurre.Log.Custom($"AutoUpdating is disabled (this is a debug build!). To force an update, use fg_debugupdate (updates debug build) or fg_forceupdate (switches build to release branch).", "FunnyGuns updater", ConsoleColor.Magenta);
            }
            else
            {
                Qurre.Log.Custom($"AutoUpdating is disabled (configured). To update, use fg_forceupdate.", "FunnyGuns updater", ConsoleColor.Magenta);
            }
        }

        /*void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Qurre.Log.Custom($"Updating, {e.ProgressPercentage}%", "FunnyGuns updater", ConsoleColor.DarkGray);
        }

        void wc_DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            Qurre.Log.Custom("Update successful! Server will restart in 5 seconds. If it does not, use softrestart!", "FunnyGuns updater", ConsoleColor.White);
            Timing.CallDelayed(5f, () => Qurre.API.Server.Restart());
        }*/
        #endregion
    }
}