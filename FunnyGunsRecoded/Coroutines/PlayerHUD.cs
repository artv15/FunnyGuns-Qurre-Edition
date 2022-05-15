using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace FunnyGunsRecoded.Coroutines
{
    public static class PlayerHUD
    {
        /// <summary>
        /// Used to display event HUD.
        /// </summary>
        /// <returns>Floats for MEC to use.</returns>
        public static IEnumerator<float> hudCoroutine()
        {
            while (Plugin.isEngaged)
            {
                if (Plugin.Stage == 0)
                {
                    foreach (var pl in Qurre.API.Player.List)
                    {
                        pl.ShowHint("\n\n\n\n\n\n\n\n\n\n" + Plugin.selectedLocale.PrepStage_HUD_START + Plugin.SecondsBeforeNextStage.ToString() + Plugin.selectedLocale.PrepStage_HUD_END, 1f);
                    }
                }
                else if (Plugin.Stage == 5) 
                {
                    foreach (var pl in Qurre.API.Player.List)
                    {
                        pl.ShowHint("\n\n\n\n\n\n\n\n\n\n\n\n" + Plugin.selectedLocale.InstantDeathHUD, 1f);
                    }
                }
                else
                {
                    //Color stuff

                    // This is not an optimal solution to this, but fine
                    // Edit: this aged quickly

                    Plugin.NTF = 0;
                    Plugin.CI = 0;

                    //Yup, this is better
                    Plugin.NTF = Qurre.API.Player.List.Count(pl => pl.Role == RoleType.NtfSergeant);
                    Plugin.CI = Qurre.API.Player.List.Count(pl => pl.Role == RoleType.ChaosRifleman);

                    /*foreach (var pl in Qurre.API.Player.List)
                    {
                        if (pl.Role == RoleType.NtfSergeant)
                        {
                            Plugin.NTF++;
                        }
                        else if (pl.Role == RoleType.ChaosRifleman)
                        {
                            Plugin.CI++;
                        }
                    }*/

                    // Plz don't beat me for that
                    // And also, why doesn't qurre have Qurre.API.Objects.Player.isNTF or .isCI field?

                    string str = $"\n\n\n\n\n\n\n\n\n\n\n\n{Plugin.selectedLocale.AlivePlayers_HUD_ALIVE} {Plugin.NTF} <color=blue>{Plugin.selectedLocale.AlivePlayers_HUD_MTF}</color>, {Plugin.CI} <color=green>{Plugin.selectedLocale.AlivePlayers_HUD_CI}</color>.\n" +
                        $"<color=orange>{Plugin.selectedLocale.Stage_HUD_CURRENTSTAGE}</color><color=" + Plugin.getStageColor() + ">" + Plugin.Stage.ToString() + $"</color>. <color=orange>{Plugin.selectedLocale.Stage_HUD_TIMEBEFORENEXTSTAGE}</color><color=green>" + (Plugin.isStageFrozen ? "<color=red>DEV Override: Stage frozen.</color>" : Plugin.SecondsBeforeNextStage.ToString()) + $"</color> {Plugin.selectedLocale.Stage_HUD_SECONDSTRANSLATION}";
                    if (Plugin.engagedMutators.Count > 0)
                    {
                        str += "\n" + Plugin.selectedLocale.CurrentMutatorsText_HUD;
                        int i = 1;
                        foreach (var m in Plugin.engagedMutators)
                        {
                            str += m.displayName;
                            if (i == Plugin.engagedMutators.Count)
                            {
                                str += ".";
                            }
                            else
                            {
                                str += ", ";
                                i++;
                            }
                        }
                    }
                    // Hud implementation
                    if (Plugin.Stage == 2)
                    {

                        str += "\n<color=" + Plugin.getWarnColor() + ">" + Plugin.selectedLocale.LCZLockdown_HUD + "</color>";
                    }
                    if (Plugin.Stage == 3)
                    {
                        str += "\n<color=" + Plugin.getWarnColor() + ">" + Plugin.selectedLocale.HCZLockdown_HUD + "</color>";
                    }

                    foreach (var pl in Qurre.API.Player.List)
                    {
                        pl.ShowHint(str, 1f);
                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
