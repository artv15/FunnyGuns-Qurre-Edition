using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;

namespace FunnyGunsRecoded.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class versionCheckClient : ICommand
    {
        public string Command => "fg_version";

        public string[] Aliases => null;

        public string Description => "Returns version of the plugin and some basic info behind updates.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var plug = new Plugin();
            response = $"\nCurrent version: V{plug.Version.Major}.{plug.Version.Minor}.{plug.Version.Build}.{plug.Version.Revision}.\nAudoupdates are {(Plugin.CustomConfig.Autoupdates ? "enabled" : "disabled")}.\n{(Plugin.IsDebugEnabled ? "This is a debug build!" : "This is a release build!")}{(Plugin.isOutdated && !Plugin.IsDebugEnabled ? "\nPlugin is outdated! (Required update from remote)" : "")}";
            return true;
        }
    }
}
