using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Newtonsoft.Json;

namespace FunnyGunsRecoded.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class audioTest : ICommand
    {
        public string Command => "fg_audiotest";

        public string[] Aliases => null;

        public string Description => "fg_audiotest";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "<b>Temporairly depricated</b>";
            return false;
        }
    }
}
