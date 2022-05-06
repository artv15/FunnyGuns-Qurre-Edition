using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FunnyGunsRecoded.Classes
{
    public class Mutator
    {
        /// <summary>
        /// Mutator class. Used to representate mutators, which are engaged during the event.
        /// </summary>
        /// <param name="_commandName">Name used for referencing during development.</param>
        /// <param name="_displayName">Name used for displaying. Expected to be `TextMeshPro'ed`.</param>
        /// <param name="_engaged">Executed upon mutator selection</param>
        /// <param name="_disengaged">Executed upon mutator deselection</param>
        /// <param name="_respawn">Executed upon player's respawn, passes through Qurre.API.Events.RoleChangeEvent (args).</param>
        /// <param name="_stageChange">Executed upon stage incrementation</param>
        public Mutator(string _commandName, string _displayName, Action _engaged, Action _disengaged, Action<Qurre.API.Events.RoleChangeEvent> _respawn, Action _stageChange)
        {
            commandName = _commandName;
            displayName = _displayName;
            engaged = _engaged;
            disengaged = _disengaged;
            respawn = _respawn;
            stageChange = _stageChange;
        }

        /// <summary>
        /// Used in commands and developement. Is not shown to player.
        /// </summary>
        public string commandName;

        /// <summary>
        /// Used in HUD. Expected to be `TextMeshPro'ed`
        /// </summary>
        public string displayName;

        /// <summary>
        /// Executed upon mutator selection
        /// </summary>
        public Action engaged;

        /// <summary>
        /// Executed upon mutator deselection
        /// </summary>
        public Action disengaged;

        /// <summary>
        /// Executed upon player's respawn, passes through Qurre.API.Events.RoleChangeEvent (args).
        /// </summary>
        public Action<Qurre.API.Events.RoleChangeEvent> respawn;

        /// <summary>
        /// Executed upon stage incrementation
        /// </summary>
        public Action stageChange;

        /// <summary>
        /// Checks, whether mutator with given commandName is engaged or not
        /// </summary>
        /// <param name="_commandName">CommandName of mutator.</param>
        /// <returns>True = Mutator is engaged, False = Mutator is not engaged.</returns>
        public static bool isEngaged(string _commandName)
        {
            foreach (var mut in Plugin.engagedMutators)
            {
                if (mut.commandName == _commandName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
