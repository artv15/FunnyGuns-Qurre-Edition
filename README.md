# FunnyGuns-Qurre-Edition
My old autoevent plugin, but now made with Qurre.

# Installation
To install FunnyGuns, you need to download the binary first! It's located in the releases.
After downloading binary, drop it in %appdata%/Qurre/Plugins (or ~/.config/Qurre/Plugins) folder. Download version 0.7.2.5 (or higher), it will auto-update to the latest one.

## Autoupdates
Autoupdates are optional, you can disable them in the config file, but I do not recommend doing it!

# How to start the event? / Commands
<ul>
  <li>fg_event start - start the event (RA)</li>
  <li>fg_event stop - stop the event (RA)</li>
  <li>fg_forceupdate - forces an update (Server Console)</li>
  <li>fg_debugupdate - forces an update for debug branch (REQUIRES APIKEY) (Server Console)</li>
</ul>

# How does this plugin work (in a nutshell)
## blah blah blah API for mutators
This is basically an API for mutators. There is a class called mutator and two lists. One named
EngagedMutators, other one named LoadedMutators. Loaded mutators is the list from which to pick mutators from.
You can even add mutator to EngagedMutators without having it being added to LoadedMutators. However, there
is usually no need to do so, because mutators are expected to be picked from loadedMutators.

## Mutator class structure
Mutator cosists of following fields:
<ol>
  <li>commandName: Used for development (engaging/disengaging from fg_override, checking if it is engaged), basically displayName, but scuffed.</li>
  <li>displayName: The name of the mutator in the engaged mutators list. Please, use TextMeshPro to color it!</li>
  <li>engaged: Action, executed upon being engaged</li>
  <li>disengaged: Action, executied upon being disengaged</li>
  <li>respawn: Action, executed upon player respawn. Gets ChangingRole eventArgs</li>
  <li>stageChange: Action, executed upon stage change</li>
</ol>
