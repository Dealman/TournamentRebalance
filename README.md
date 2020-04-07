# Tournament Rebalance
This mod aims to rebalance Tournament and Arena Practice by introducing combat XP rewards, using the same formula as normal combat - but reduced by 50%.

With the release of 1.0.6 however, this feature of the mod has been disabled but left in the code for educational purposes should someone find it useful.

It still increases the renown gained as well as introduces a new mechanic for receiving a bit of gold as well for winning.

## XP Rewards
Uses the same formula as normal combat XP gains, using the Combat XP Model to calculate the XP. This XP however is reduced by 50%, I thought this felt _"realistic"_ as it's a fight without any real drawbacks. Whereas actual combat has obvious risks, thus yields more XP.

Disabled as of version 1.0.1 due to Bannerlord e1.0.6!

## Renown Rewards
This mod changes the way renown is rewarded by winning tournaments. By default, you would only win a mere 3.0 renown no matter how good or bad you performed. No matter whom you beat to the ground. I thought this was really silly and made tournaments feel lackluster and unimportant.

I have introduced a mechanic here which rewards you with renown based on a few factors such as your amount of kills and _who_ you killed. This is how it works as of 1.0.1:

**Non-Hero** _(Normal recruitable soldiers, looters and etc)_
|Tier|Renown|
|---|---|
|1|+1|
|2|+1|
|3|+1|
|4|+2|
|5|+2|

**Hero** _(People of some renown such as leaders, commanders and etc)_
|Flag|Renown|
|---|---|
|Hero|+1|
|Noble|+3|
|Notable|+1|
|Commander|+1|
|Minor Faction Leader|+5|
|Faction Leader|+10|

A special case for opponents that were some kind of hero is that the renown stacks. So if you defeat someone that is both a Noble and a Commander - you would earn a bonus 5.0 renown _(+1, +3, +1)_. This should make defeating more renowned characters feel a bit more impactful.

# Installation
Unzip the contents into your Modules folder. It should look something like this;  
```C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\TournamentRebalance```

Then you can enable it either via the game launcher or by creating a shortcut to Bannerlord.exe and adding these parameters to the target;  
```/singleplayer _MODULES_*Native*SandBox*SandBoxCore*StoryMode*CustomBattle*TournamentRebalance*_MODULES_```

# Overriden Models and Patches
## Overriden Models
This mod overrides the ```"OnRenownReward"``` method in the ```DefaultTournamentModel``` class to return a custom amount of renown instead of a constant value.

## Patched Classes
```"OnPlayerWinTournament"``` method in the ```TournamentBehavior``` class. Postfix.
```"OnPlayerEliminated"``` method in the ```TournamentBehavior``` class. Postfix.
```"OnAgentRemoved"``` method in the ```TournamentFightMissionController``` class. Postfix.
```"OnTournamentEnd"``` method in the ```TournamentVM``` class. Postfix
