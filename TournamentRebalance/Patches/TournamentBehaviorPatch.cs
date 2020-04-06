using HarmonyLib;
using SandBox.TournamentMissions.Missions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TournamentRebalance.Patches
{
    // We wanna patch the "OnPlayerWinTournament" method in the TournamentBehavior class
    // By doing this we can give the player gold if they won the tournament (or anything else, really, like items/xp)
    // While also leaving the bettings earned entry untouched
    [HarmonyPatch(typeof(TournamentBehavior), "OnPlayerWinTournament")]
    public class TournamentBehaviorPatch
    {
        static void Postfix(TournamentBehavior __instance)
        {
            if (Campaign.Current.GameMode != CampaignGameMode.Campaign)
                return;

            RebalancedTournamentModel rebalancedModel = GetRebalancedTournamentModel();
            if(rebalancedModel != null)
            {
                if(rebalancedModel.DenarsFromKills > 0)
                {
                    //__instance.OverallExpectedDenars = __instance.OverallExpectedDenars + rebalancedModel.DenarsFromKills;
                    GiveGoldAction.ApplyBetweenCharacters((Hero)null, Hero.MainHero, rebalancedModel.DenarsFromKills, true);
                    InformationManager.DisplayMessage(new InformationMessage($"You receive an additional {rebalancedModel.DenarsFromKills}<img src=\"Icons\\Coin@2x\"> for beating {rebalancedModel.DenarsFromKills/100} opponents.", "event:/ui/notification/coins_positive"));
                }
            }
        }

        static bool Prepare()
        {
            return true;
        }

        static RebalancedTournamentModel GetRebalancedTournamentModel()
        {
            var gameModels = Campaign.Current.Models.GetGameModels();

            // We loop through them in reverse because modded models are usually, if not always - last. Just a minor optimization
            // Might probably be a better way of doing it than this, but it works...
            for (int i = gameModels.Count; i-- > 0;)
            {
                RebalancedTournamentModel rebalancedModel = gameModels[i] as RebalancedTournamentModel;
                if (rebalancedModel != null)
                {
                    return rebalancedModel;
                }
            }
            return null;
        }
    }
}
