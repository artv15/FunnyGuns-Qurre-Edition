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

        public string Description => "FunnyGuns event info";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = Plugin.selectedLocale.InfoCommandText;
            return true;
        }
    }
}
