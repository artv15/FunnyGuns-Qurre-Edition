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
        public override System.Version Version { get; } = new System.Version(0, 7, 0, 4); /* <- plugin version(optional) */
        public static Config? CustomConfig { get; private set; } /* <- creating a new config class */

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
        /// Amount of MTF, assigned by HUD Coroutine
        /// </summary>
        public static int NTF = 0;

        /// <summary>
        /// Mutators, which are currently engaged (they are impacting the game rn)
        /// </summary>
        public static List<Classes.Mutator> engagedMutators = new();

        /// <summary>
        /// Mutators, which are currently loaded (they are picked during the game (stage incrementation))
        /// </summary>
        public static List<Classes.Mutator> loadedMutators = new();

        
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

        [Obsolete("No longer in use, due to mutator being non-existant!")]
        public static bool isAmmoInfinite = true;
        #endregion

        #region Enable/Disable
        public override void Enable()
        {
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
    }
}