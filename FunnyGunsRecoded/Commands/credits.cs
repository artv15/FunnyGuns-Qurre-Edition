using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;

namespace FunnyGunsRecoded.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class credits : ICommand
    {
        public string Command => "fg_info";

        public string[] Aliases => null;

        public string Description => "Выдаёт информацию и разработчиков Funny Guns";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "\n<color=green>[ Funny Guns => info ]</color>\n" +
                "<color=green>-- Разработчики --</color>\n" +
                "<color=yellow>Treeshold#0001 (aka Star Butterfly) - Разработчик</color>\n" +
                "<color=yellow>Dlorka#9909 (aka Tushkanchik) - Тестер</color>\n\n" +
                "<color=green>-- Об ивенте --</color>\n" +
                "<color=yellow>Противостояние двух сторон, хаоса и мога. Задача каждого - истребить вражескую команду.</color>\n" +
                "<color=yellow>Во время ивента, некоторые механики будут меняться. Например, у всех будет рентгеновское зрение.</color>\n\n" +
                "<color=grey>Да выживет сильнейшая команда!</color>";
            return true;
        }
    }
}
