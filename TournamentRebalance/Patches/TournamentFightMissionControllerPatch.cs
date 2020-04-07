using HarmonyLib;
using SandBox;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace TournamentRebalance.Patches
{
    [HarmonyPatch(typeof(TournamentFightMissionController), "OnAgentRemoved")]
    public class TournamentFightMissionControllerPatch
    {
        public int playerKills = 0;

        static void Postfix(TournamentFightMissionController __instance, Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
        {
            if (affectorAgent == null || affectedAgent == null)
                return;
            if (affectorAgent.Character == null || affectedAgent.Character == null)
                return;

            if (affectorAgent.Character.IsPlayerCharacter)
            {
                CharacterObject killedEnemy = (CharacterObject)affectedAgent.Character;
                if(killedEnemy.HeroObject != null)
                {
                    int killRenownWorth = 1;

                    bool isNoble = killedEnemy.HeroObject.IsNoble;
                    bool isNotable = killedEnemy.HeroObject.IsNotable;
                    bool isCommander = killedEnemy.HeroObject.IsCommander;
                    bool isMinorFactionLeader = killedEnemy.HeroObject.IsMinorFactionHero;
                    bool isFactionLeader = killedEnemy.HeroObject.IsFactionLeader;

                    killRenownWorth = (isNoble) ? killRenownWorth + 3 : killRenownWorth;
                    killRenownWorth = (isNotable) ? killRenownWorth + 1 : killRenownWorth;
                    killRenownWorth = (isCommander) ? killRenownWorth + 1 : killRenownWorth;
                    killRenownWorth = (isMinorFactionLeader) ? killRenownWorth + 5 : killRenownWorth;
                    killRenownWorth = (isFactionLeader) ? killRenownWorth + 10 : killRenownWorth;

                    RebalancedTournamentModel rebalancedTournamentModel = GetRebalancedTournamentModel();
                    if (rebalancedTournamentModel != null)
                    {
                        rebalancedTournamentModel.RenownFromKills = rebalancedTournamentModel.RenownFromKills + killRenownWorth;
                        rebalancedTournamentModel.DenarsFromKills = rebalancedTournamentModel.DenarsFromKills + 100;
                    } else {
                        InformationManager.DisplayMessage(new InformationMessage("[TournamentRebalance]: Error, GetRebalancedTournamentModel() returned null!", Color.ConvertStringToColor("#FF0000FF")));
                    }
                } else {
                    int killRenownWorth = 0;

                    switch (killedEnemy.Tier)
                    {
                        case 1:
                            killRenownWorth = 1;
                            break;
                        case 2:
                            killRenownWorth = 1;
                            break;
                        case 3:
                            killRenownWorth = 1;
                            break;
                        case 4:
                            killRenownWorth = 2;
                            break;
                        case 5:
                            killRenownWorth = 2;
                            break;
                        default:
                            killRenownWorth = 1;
                            break;
                    }

                    RebalancedTournamentModel rebalancedTournamentModel = GetRebalancedTournamentModel();
                    if(rebalancedTournamentModel != null)
                    {
                        rebalancedTournamentModel.RenownFromKills = rebalancedTournamentModel.RenownFromKills + killRenownWorth;
                        rebalancedTournamentModel.DenarsFromKills = rebalancedTournamentModel.DenarsFromKills + 100;
                    } else {
                        InformationManager.DisplayMessage(new InformationMessage("[TournamentRebalance]: Error, GetRebalancedTournamentModel() returned null!", Color.ConvertStringToColor("#FF0000FF")));
                    }
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
