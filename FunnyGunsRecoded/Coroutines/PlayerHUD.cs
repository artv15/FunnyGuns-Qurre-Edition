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
                        pl.ShowHint("\n\n\n\n\n\n\n\n\n\n\n\n<color=yellow>Стадия подготовки. До окончания подготовки осталось </color><color=green>" + Plugin.SecondsBeforeNextStage.ToString() + "</color> секунд(ы)" + "\n" +
                            "<color=green>Суть ивента</color>: Ваша задача истребить вражескую команду. Во время игры, будут добавляться мутаторы. Они меняют некоторые правила игры.\n" +
                            "<color=blue>Если вы хотите узнать имена разработчиков, то используйте</color> <color=red>.fg_info</color><color=blue> в консоли (~)</color>", 1f);
                    }
                }
                else if (Plugin.Stage == 4) 
                {
                    foreach (var pl in Qurre.API.Player.List)
                    {
                        pl.ShowHint("\n\n\n\n\n\n\n\n\n\n\n\n<color=red>Внезапная смерть!</color>\n<color=yellow>Все живые игроки будут терять по 2 хп в секунду. Уничтожьте вражескую команду или останьтесь последней в живых.</color>", 1f);
                    }
                }
                else
                {
                    string color = "red";
                    switch (Plugin.Stage)
                    {
                        case 1:
                            color = "green";
                            break;
                        case 2:
                            color = "yellow";
                            break;
                        case 3:
                            color = "red";
                            break;
                    }

                    // This is not an optimal solution to this, but fine

                    Plugin.NTF = 0;
                    Plugin.CI = 0;

                    foreach (var pl in Qurre.API.Player.List)
                    {
                        if (pl.Role == RoleType.NtfSergeant)
                        {
                            Plugin.NTF++;
                        }
                        else if (pl.Role == RoleType.ChaosRifleman)
                        {
                            Plugin.CI++;
                        }
                    }

                    // Plz don't beat me for that
                    // And also, why doesn't qurre have Qurre.API.Objects.Player.isNTF or .isCI field?

                    string str = $"\n\n\n\n\n\n\n\n\n\n\n\nЖивы: " + Plugin.NTF.ToString() + "<color=blue>Моговцев </color>, " + Plugin.CI.ToString() + " <color=green>Хаоситов </color>\n<color=orange>Текущая стадия: </color><color=" + color + ">" + Plugin.Stage.ToString() + "</color>. <color=orange>До следующей стадии </color><color=green>" + (Plugin.isStageFrozen ? "<color=red>DEV Override: Stage frozen.</color>" : Plugin.SecondsBeforeNextStage.ToString()) + "</color> секунд(ы)";
                    if (Plugin.engagedMutators.Count > 0)
                    {
                        str += "\nТекущие мутаторы: ";
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
