using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Library;

namespace TournamentRebalance
{
    public class RebalancedTournamentModel : DefaultTournamentModel
    {
        // Extra Denars from Kills
        private int denarsFromKills = 0;
        public int DenarsFromKills {
            get { return denarsFromKills; }
            set { denarsFromKills = MBMath.ClampInt(value, 0, 65535); }
        }
        // Extra Renown from Kills
        private int renownFromKills = 0;
        public int RenownFromKills
        {
            get { return renownFromKills; }
            set { renownFromKills = MBMath.ClampInt(value, 0, 65535); }
        }

        public override int GetRenownReward(Hero winner, Town town)
        {
            /*
            Hero playerHero = Hero.MainHero;
            if(playerHero != null)
            {
                if(town == playerHero.CurrentSettlement.Town)
                {
                    int renown = renownFromKills;
                    renownFromKills = 0;
                    // Try counter method again
                    // Find event for when player leaves tournament or town

                    return renown;
                }
            }
            */

            return renownFromKills;
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
