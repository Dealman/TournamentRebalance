using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

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
            return RenownFromKills;
        }
    }
}
