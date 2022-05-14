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
                        pl.ShowHint("\n\n\n\n\n\n\n\n\n\n<color=yellow>Стадия подготовки. До окончания подготовки осталось </color><color=green>" + Plugin.SecondsBeforeNextStage.ToString() + "</color> секунд(ы)" + "\n" +
                            "<color=green>Суть ивента</color>: Ваша задача истребить вражескую команду. Во время игры, будут добавляться мутаторы. Они меняют некоторые правила игры.\n" +
                            "<color=green>Во время подготовки лифты не могут быть вызваны.</color>\n" +
                            "<color=red>Специально для людей, которые будут говорить, что это говно-ивент. Если вы уж это и делаете, то говорите хотя-бы почему.</color>\n" +
                            //"<color=red>Если вы хотите узнать больше(что вряд-ли), то .fg_govnoivent в консоль (~)</color>\n" +
                            "<color=blue>Если вы хотите узнать имена разработчиков, то используйте</color> <color=red>.fg_info</color><color=blue> в консоли (~)</color>", 1f);
                    }
                }
                else if (Plugin.Stage == 5) 
                {
                    foreach (var pl in Qurre.API.Player.List)
                    {
                        pl.ShowHint("\n\n\n\n\n\n\n\n\n\n\n\n<color=red>Внезапная смерть!</color>\n<color=yellow>Все живые игроки будут терять по 2 хп в секунду. Уничтожьте вражескую команду или останьтесь последней в живых.</color>", 1f);
                    }
                }
                else
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

                    // This is not an optimal solution to this, but fine
                    // Edit: this aged quickly

                    Plugin.NTF = 0;
                    Plugin.CI = 0;

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
                    // Hud implementation
                    if (Plugin.Stage == 2)
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
                        str += "\n<color=" + colorAlert + ">Закрытие лайт зоны в начале следующей стадии!</color>";
                    }
                    if (Plugin.Stage == 3)
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
                        str += "\n<color=" + colorAlert + ">Закрытие хард зоны в начале следующей стадии!</color>";
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
