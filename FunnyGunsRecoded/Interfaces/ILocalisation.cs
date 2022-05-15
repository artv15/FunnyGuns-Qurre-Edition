using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyGunsRecoded.Interfaces
{
    public interface ILocalisation
    {
        string PrepStage_HUD_START { get; set; }
        string PrepStage_HUD_END { get; set; }
        string InstantDeathHUD { get; set; }
        string AlivePlayers_HUD_ALIVE { get; set; }
        string AlivePlayers_HUD_MTF { get; set; }
        string AlivePlayers_HUD_CI { get; set; }
        string Stage_HUD_CURRENTSTAGE { get; set; }
        string Stage_HUD_TIMEBEFORENEXTSTAGE { get; set; }
        string Stage_HUD_SECONDSTRANSLATION { get; set; }

        string LCZLockdown_HUD { get; set; }
        string HCZLockdown_HUD { get; set; }
        string CurrentMutatorsText_HUD { get; set; }

        string TutorialAssaultBaseName { get; set; }
        string TutorialAssaultPrep { get; set; }
        string TutorialAssaultAnticipation { get; set; }
        string TutorialAssaultAssault { get; set; }
        string TutorialAssaultFade { get; set; }
        string EventEnd { get; set; }
        string EventEnd_CI_WIN { get; set; }
        string EventEnd_MTF_WIN { get; set; }
        string LocdownDeathReason { get; set; }
        void OnInit();
    }
}
