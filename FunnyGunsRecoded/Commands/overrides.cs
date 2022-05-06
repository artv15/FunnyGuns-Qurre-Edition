using CommandSystem;

namespace FunnyGunsRecoded.Commands
{
#if DEBUG
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class overrides : ICommand
    {
        public string Command => "fg_override";

        public string[]? Aliases => null;

        public string Description => "Used for development!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.IsDebugEnabled)
            {
                var args = arguments.Array;
                if (args.Length == 1)
                {
                    response = "Used for development.";
                    return true;
                }
                else
                {
                    switch (args[1].ToLower())
                    {
                        case "players":
                            Plugin.checkForEndgame = false;
                            response = "Done! Endgame checker is no longer running!";
                            return true;
                            break;
                        case "mutator":
                            if (args.Length == 4)
                            {
                                switch (args[2].ToLower())
                                {
                                    case "enable":
                                        bool found = false;
                                        foreach (var mut in Plugin.loadedMutators)
                                        {
                                            if (mut.commandName == args[3])
                                            {
                                                found = true;
                                                Plugin.engagedMutators.Add(mut);
                                                mut.engaged.Invoke();
                                                break;
                                            }
                                        }
                                        if (!found)
                                        {
                                            response = "Mutator not found.";
                                            return false;
                                        }
                                        else
                                        {
                                            response = "Done!";
                                            return true;
                                        }
                                        break;
                                    case "disable":
                                        bool foundd = false;
                                        foreach (var mut in Plugin.loadedMutators)
                                        {
                                            if (mut.commandName == args[3])
                                            {
                                                found = true;
                                                Plugin.engagedMutators.Remove(mut);
                                                mut.disengaged.Invoke();
                                            }
                                        }
                                        if (!foundd)
                                        {
                                            response = "Mutator not found.";
                                            return false;
                                        }
                                        else
                                        {
                                            response = "Done!";
                                            return true;
                                        }
                                        break;
                                    case "list":
                                        response = "Loaded Mutators: \n";
                                        foreach (var mut in Plugin.loadedMutators)
                                        {
                                            response = response + "DevName: " + mut.commandName + ",  Name: " + mut.displayName + ".\n";
                                        }
                                        return true;
                                        break;
                                    default:
                                        response = "use enable/disable as args[2]";
                                        return false;
                                        break;
                                }
                            }
                            else
                            {
                                response = "Need only 3 arguments!";
                                return false;
                            }
                            break;
                        case "freezestage":
                            if (Plugin.isStageFrozen)
                            {
                                Plugin.isStageFrozen = false;
                                response = "Unfroze the stage!";
                                return true;
                            }
                            else
                            {
                                Plugin.isStageFrozen = true;
                                response = "Froze the stage!";
                                return true;
                            }
                            break;
                        default:
                            response = "Invalid Subcommand!";
                            return false;
                            break;
                    }
                }
            }
            else
            {
                response = "<color=red>It's not a debug build.</color>";
                return false;
            }
        }
    }
#endif
}
