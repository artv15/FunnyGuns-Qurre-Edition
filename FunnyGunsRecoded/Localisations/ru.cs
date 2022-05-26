using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyGunsRecoded.Localisations
{
    public class ru : Interfaces.ILocalisation
    {
        public string PrepStage_HUD_START { get; set; } = "<color=yellow> Стадия подготовки.До окончания подготовки осталось</color><color=green> ";
        public string PrepStage_HUD_END { get; set; } = "</color> секунд(ы)" + "\n" +
                            "<color=green>Суть ивента</color>: Ваша задача истребить вражескую команду. Во время игры, будут добавляться мутаторы. Они меняют некоторые правила игры.\n" +
                            "<color=green>Во время подготовки лифты не могут быть вызваны.</color>\n" +
                            "<color=red>Специально для людей, которые будут говорить, что это говно-ивент. Если вы уж это и делаете, то говорите хотя-бы почему.</color>\n" +
                            //"<color=red>Если вы хотите узнать больше(что вряд-ли), то .fg_govnoivent в консоль (~)</color>\n" +
                            "<color=blue>Если вы хотите узнать имена разработчиков, то используйте</color> <color=red>.fg_info</color><color=blue> в консоли (~)</color>";
        public string InstantDeathHUD { get; set; } = "<color=red>Внезапная смерть!</color>\n<color=yellow>Все живые игроки будут терять по 4 хп в секунду. Уничтожьте вражескую команду или останьтесь последней в живых.</color>";
        public string LCZLockdown_HUD { get; set; } = "Закрытие лайт зоны в начале следующей стадии!";
        public string HCZLockdown_HUD { get; set; } = "Закрытие хард зоны в начале следующей стадии!";
        public string CurrentMutatorsText_HUD { get; set; } = "Текущие мутаторы: ";
        public string TutorialAssaultBaseName { get; set; } = "Штурм туториалов";
        public string TutorialAssaultPrep { get; set; } = "Подготовка";
        public string TutorialAssaultAnticipation { get; set; } = "Антисипация";
        public string TutorialAssaultAssault { get; set; } = "В прогрессе";
        public string TutorialAssaultFade { get; set; } = "Стихание";
        public string EventEnd { get; set; } = "<color=green>Ивент окончен.</color> Победа за ";
        public string EventEnd_CI_WIN { get; set; } = "<color=green>Хаосом</color>";
        public string EventEnd_MTF_WIN { get; set; } = "<color=blue>MTF</color>";


        public string AlivePlayers_HUD_ALIVE { get; set; } = "Живы: ";
        public string AlivePlayers_HUD_MTF { get; set; } = "Моговцев";
        public string AlivePlayers_HUD_CI { get; set; } = "Хаоситов";
        public string LocdownDeathReason { get; set; } = "Зона была отсечена";
        public string Stage_HUD_CURRENTSTAGE { get; set; } = "Текущая стадия: ";
        public string Stage_HUD_TIMEBEFORENEXTSTAGE { get; set; } = "До следующей стадии ";
        public string Stage_HUD_SECONDSTRANSLATION { get; set; } = "секунд(ы)";
        public string InfoCommandText { get; set; } = "\n<color=green>[ Funny Guns => info ]</color>\n" +
                "<color=green>-- Разработчики --</color>\n" +
                "<color=yellow>Treeshold#0001 (aka Star Butterfly) - Разработчик</color>\n" +
                "<color=yellow>Dlorka#9909 (aka Tushkanchik) - Тестер</color>\n\n" +
                "<color=green>-- Об ивенте --</color>\n" +
                "<color=yellow>Противостояние двух сторон, хаоса и мога. Задача каждого - истребить вражескую команду.</color>\n" +
                "<color=yellow>Во время ивента, некоторые механики будут меняться. Например, у всех будет рентгеновское зрение.</color>\n\n" +
                "<color=grey>Да выживет сильнейшая команда!</color>";
        public string StormDeathReason { get; set; } = "Вы замёрзли до смерти";
        public string StormBroadcastText { get; set; } = "<color=#6cd4dd>На улице слишком холодно! Идите в комплекс немедленно!</color>";
        public void OnInit()
        {
            Plugin.MutatorLocaleDict.Add("passiveRegen", "<color=green>Пассивная регенерация</color>");
            Plugin.MutatorLocaleDict.Add("fogOfWar", "<color=orange>Густой туман</color>");
            Plugin.MutatorLocaleDict.Add("lightsOut", "<color=orange>Нет света</color>");
            Plugin.MutatorLocaleDict.Add("speed++", "<color=green>Скорость передвижения увеличена!</color>");
            Plugin.MutatorLocaleDict.Add("noTeslaGates", "<color=orange>Тесла ворота отключены</color>");
            Plugin.MutatorLocaleDict.Add("legalWH", "<color=green>Рентгеновское зрение</color>");
            Plugin.MutatorLocaleDict.Add("bleeding", "<color=red>Кровотечение от огнестрельных ранений</color>");
            Plugin.MutatorLocaleDict.Add("badBullets", "<color=red>Некоторые патроны - холостые</color>");
            Plugin.MutatorLocaleDict.Add("theInevitable", "<color=#6cd4dd>Буря на поверхности</color>");
            Plugin.MutatorLocaleDict.Add("poob", "<color=#6cd4dd>#poob навсегда</color>");
        }
    }
}
