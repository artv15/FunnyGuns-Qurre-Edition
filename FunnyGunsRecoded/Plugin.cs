using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public override System.Version Version { get; } = new System.Version(0, 7, 1, 0); /* <- plugin version(optional) */
        public Config CustomConfig { get; private set; } /* <- creating a new config class */

#if DEBUG
        public static bool IsDebugEnabled = true;
#else
        public static bool IsDebugEnabled = false;
#endif

        #endregion

        #region globals
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
            checkVersion();
            CustomConfig = new Config();
            CustomConfigs.Add(CustomConfig);
            ev = new Events();
            Qurre.Events.Player.Dies += ev.playerDied;
            Qurre.Events.Player.RoleChange += ev.roleChanging;
            Qurre.Events.Round.Waiting += ev.WaitingForPlayers;
            Qurre.Events.Player.DamageProcess += ev.hurting;
            Qurre.Events.Player.RechargeWeapon += ev.onReloading;
            Qurre.Events.Round.TeamRespawn += ev.TeamRespawn;
            Qurre.Events.Player.DropAmmo += ev.droppingAmmo;
        }

        public override void Disable()
        {
            CustomConfigs.Clear();
            Qurre.Events.Player.Dies -= ev.playerDied;
            Qurre.Events.Player.RoleChange -= ev.roleChanging;
            Qurre.Events.Round.Waiting -= ev.WaitingForPlayers;
            Qurre.Events.Player.DamageProcess -= ev.hurting;
            Qurre.Events.Player.RechargeWeapon -= ev.onReloading;
            Qurre.Events.Round.TeamRespawn -= ev.TeamRespawn;
            Qurre.Events.Player.DropAmmo -= ev.droppingAmmo;
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

        void checkVersion()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string JSON_raw = wc.DownloadString("https://treesholdapi.ml/FunnyGuns/version.php");
                    version verRemote = JsonSerializer.Deserialize<version>(JSON_raw);
                    bool isOutdated = true;
                    Plugin pl = new Plugin();
                    if (this.Version.Major < verRemote.major)
                    {
                        Qurre.Log.Error($"WARNING! Your version of FunnyGuns is one major version behind! Please, UPDATE THE PLUGIN FOR GOD'S SAKE!\nYour version: {this.Version.Major}.{this.Version.Minor}.{this.Version.Build}.{this.Version.Revision}; " +
                            $"Remote version: {verRemote.major}.{verRemote.minor}.{verRemote.patch}.{verRemote.revision}");
                    }
                    else if (this.Version.Minor < verRemote.minor && this.Version.Major >= verRemote.major)
                    {
                        Qurre.Log.Warn($"Hey! Your version of FunnyGuns is one minor version behind! Update the plugin if you want to!\nYour version: {this.Version.Major}.{this.Version.Minor}.{this.Version.Build}.{this.Version.Revision}; " +
                            $"Remote version: {verRemote.major}.{verRemote.minor}.{verRemote.patch}.{verRemote.revision}");
                    }
                    else if (this.Version.Build < verRemote.patch && this.Version.Minor >= verRemote.minor && this.Version.Major >= verRemote.major)
                    {
                        Qurre.Log.Info($"Hey! Your version of FunnyGuns is one patch/build version behind! There is no need to update the plugin right away, however, I recommend to do so.\nYour version: {this.Version.Major}.{this.Version.Minor}.{this.Version.Build}.{this.Version.Revision}; " +
                            $"Remote version: {verRemote.major}.{verRemote.minor}.{verRemote.patch}.{verRemote.revision}");
                    }
                    else if (this.Version.Revision < verRemote.revision && this.Version.Build >= verRemote.patch && this.Version.Minor >= verRemote.minor && this.Version.Major >= verRemote.major)
                    {
                        Qurre.Log.Info($"Psst! Your version of FunnyGuns is one revision version behind! Update if you want so!\nYour version: {this.Version.Major}.{this.Version.Minor}.{this.Version.Build}.{this.Version.Revision}; " +
                            $"Remote version: {verRemote.major}.{verRemote.minor}.{verRemote.patch}.{verRemote.revision}");
                    }
                    else if (Plugin.IsDebugEnabled)
                    {
                        Qurre.Log.Info("This is a debug build. It will not be updated nor the version will be checked!");
                    }
                    else
                    {
                        isOutdated = false;
                        Qurre.Log.Info("FunnyGuns is up to date!");
                    }
                    if (isOutdated)
                    {
                        Qurre.Log.Info("Plugin is outdated, will be trying to update it rn");
                        try
                        {
                            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                            wc.DownloadFileCompleted += wc_DownloadComplete;
                            wc.DownloadFileAsync(new Uri("https://treesholdapi.ml/FunnyGuns/FunnyGunsRecoded.dll"), Qurre.PluginManager.PluginsDirectory + "/FunnyGunsRecoded.dll");
                        }
                        catch (Exception ex)
                        {
                            Qurre.Log.Warn($"Autoupdate failed! Error: {ex.Message}. Try using fg_forceupdate");
                        }
                        //Qurre.Log.Info($"version (remote said): {verRemote.major}.{verRemote.minor}.{verRemote.patch}.{verRemote.revision}");
                    }
                }
            }
            catch (Exception ex)
            {
                Qurre.Log.Warn($"Failed to check for updates! There seems to be an issue with the remote! Error: {ex.Message}. Stack trace: {ex.StackTrace}");
            }
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
        #endregion


    }
}