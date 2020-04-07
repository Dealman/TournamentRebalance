using HarmonyLib;
using SandBox.ViewModelCollection.Tournament;
using TaleWorlds.CampaignSystem;

namespace TournamentRebalance.Patches
{
    [HarmonyPatch(typeof(TournamentVM), "OnTournamentEnd")]
    public class TournamentVMPatch
    {
        static void Postfix(TournamentVM __instance)
        {
            /*
            *   As it turns out GetRenownReward() which was overriden in RebalancedTournamentModel is executed for simulated tournaments
            *   as well as several times when a tournament end(?). Thus resetting the variable in there was not a viable option.
            *   Managed to find this method and it serves the purpose perfectly and has a lot of properties we could change/use if we wanted to
            */
            if (__instance.TournamentWinner.Participant.Character.IsPlayerCharacter)
            {
                RebalancedTournamentModel rebalancedTournamentModel = GetRebalancedTournamentModel();
                rebalancedTournamentModel.DenarsFromKills = 0;
                rebalancedTournamentModel.RenownFromKills = 0;
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
