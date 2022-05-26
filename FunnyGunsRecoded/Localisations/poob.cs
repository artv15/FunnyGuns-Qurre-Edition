using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyGunsRecoded.Localisations
{
    public class poob : Interfaces.ILocalisation
    {
        public string PrepStage_HUD_START { get; set; } = "<color=yellow>poob. poob in</color><color=green> ";
        public string PrepStage_HUD_END { get; set; } = "</color> poob(s)" + "\n" +
                            "<color=green>poob poob poob poob poob</color>: poob poob poob poob. poob poob poob, poob poob poob poob poobed. poob poob poob poob poob poob poob\n" +
                            "<color=green>poob poob poob poob poob poob</color>\n" +
                            "<color=blue>poob poob poob poob poob poob poob poob at .poob (~ poob)</color>";
        public string InstantDeathHUD { get; set; } = "<color=red>poob!</color>\n<color=yellow>poob poobs poob poob poobed poob 4 poobs poob poob! poob poob poobs poob poob poob poob poob poob poob!</color>";
        public string AlivePlayers_HUD_ALIVE { get; set; } = "poob: ";
        public string AlivePlayers_HUD_MTF { get; set; } = "poobs";
        public string AlivePlayers_HUD_CI { get; set; } = "poobs";
        public string Stage_HUD_CURRENTSTAGE { get; set; } = "poob poob: ";
        public string Stage_HUD_TIMEBEFORENEXTSTAGE { get; set; } = "poob poob poob poob ";
        public string Stage_HUD_SECONDSTRANSLATION { get; set; } = "poob(s)";
        public string LCZLockdown_HUD { get; set; } = "poob poob poob poob poob poob!";
        public string HCZLockdown_HUD { get; set; } = "poob poob poob poob poob poob!";
        public string CurrentMutatorsText_HUD { get; set; } = "poob poob: ";
        public string TutorialAssaultBaseName { get; set; } = "Tutorial poob";
        public string TutorialAssaultPrep { get; set; } = "poob";
        public string TutorialAssaultAnticipation { get; set; } = "poob";
        public string TutorialAssaultAssault { get; set; } = "poob";
        public string TutorialAssaultFade { get; set; } = "poob";
        public string EventEnd { get; set; } = "<color=green>poob poobed.</color> poob poob poob";
        public string EventEnd_CI_WIN { get; set; } = "<color=green>poob</color>";
        public string EventEnd_MTF_WIN { get; set; } = "<color=blue>poob</color>";
        public string LocdownDeathReason { get; set; } = "poob poob poobed";
        public string InfoCommandText { get; set; } = "\n<color=green>[ Funny Guns => poob ]</color>\n" +
                "<color=green>-- poobs --</color>\n" +
                "<color=yellow>Treeshold#0001 (aka Star Butterfly) - Coder poob</color>\n" +
                "<color=yellow>Dlorka#9909 (aka Tushkanchik) - poob's worst nightmare</color>\n\n" +
                "<color=green>-- About --</color>\n" +
                "<color=yellow>poob deathmatch. Your goal is to eliminate the poobs.</color>\n" +
                "<color=yellow>During the event, poobs will be added, they will alter the poob.</color>\n\n" +
                "<color=grey>Let the strongest poobs win!</color>";
        public string StormDeathReason { get; set; } = "poob poob poobed poob poob.";
        public string StormBroadcastText { get; set; } = "<color=#6cd4dd>poob poob pooby! poob poob poob poob poob!</color>";
        public void OnInit()
        {
            //no inits 4 u
            //Plugin.MutatorLocaleDict.Add("", "");
        }
    }
}
