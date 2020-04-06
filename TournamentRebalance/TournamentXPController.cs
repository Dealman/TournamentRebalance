using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace TournamentRebalance
{
    public class TournamentXPController : MissionLogic
    {
        private float xpGainMultiplier = 0.50f;

        // We use OnScoreHit to track whenever the player scores a hit and reward XP using the same formula as normal battles, but (by default) reduce it by 50%
        public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, int affectorWeaponKind, bool isBlocked, float damage, float movementSpeedDamageModifier, float hitDistance, AgentAttackType attackType, float shotDifficulty, int weaponCurrentUsageIndex)
        {
            base.OnScoreHit(affectedAgent, affectorAgent, affectorWeaponKind, isBlocked, damage, movementSpeedDamageModifier, hitDistance, attackType, shotDifficulty, weaponCurrentUsageIndex);

            // Make sure we have valid references to work with, otherwise return
            if (affectorAgent == null || affectorAgent.Character == null || affectedAgent.Character == null)
                return;

            // Prevents rewarding XP for damage higher than the health limit
            // Example: Target has 100 health and you hit them for 250 damage, only 100 damage is considered for XP gain instead of the full 250 damage to prevent massive XP spikes
            if ((double)damage > (double)affectedAgent.HealthLimit)
                damage = affectedAgent.HealthLimit;

            // Only continue if the affectorAgent(instigator) is the player character and if the target is human
            if (affectorAgent.Character.IsPlayerCharacter && affectedAgent.IsHuman && affectedAgent != Agent.Main)
            {
                CharacterObject playerCharacter = (CharacterObject)affectorAgent.Character;
                CharacterObject enemyCharacter = (CharacterObject)affectedAgent.Character;
                if (playerCharacter == null || enemyCharacter == null)
                    // For some reason either of the characters returned null, abort to prevent crashing
                    return;

                Hero hero = playerCharacter.HeroObject;

                bool isFatal = ((double)affectedAgent.Health - (double)damage < 1.0);
                bool affectorMounted = (affectorAgent.MountAgent != null);
                bool isTeamkill = (affectedAgent.Team.Side == affectorAgent.Team.Side);
                int xpAmount = 0;

                // Use the current CombatXpModel to calculate the XP the hit should've provided
                Campaign.Current.Models.CombatXpModel.GetXpFromHit(playerCharacter, enemyCharacter, (int)damage, isFatal, false, out xpAmount);

                // Try and fetch what kind of weapon was used
                ItemObject itemFromWeaponKind = ItemObject.GetItemFromWeaponKind(affectorWeaponKind);
                if (itemFromWeaponKind != null)
                {
                    // Fetch the skill that is used for the weapon(OneHanded, Bow, etc)
                    SkillObject skillForWeapon = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(itemFromWeaponKind, weaponCurrentUsageIndex);

                    // If the player used a bow, the skill should be calculated a bit differently taking shot difficulty into account
                    if (skillForWeapon == DefaultSkills.Bow)
                    {
                        float xpMultiplier = 0.5f;
                        if ((double)shotDifficulty > 0.0)
                        {
                            float newXpAmount = (float)xpAmount * xpMultiplier * Campaign.Current.Models.CombatXpModel.GetXpMultiplierFromShotDifficulty(shotDifficulty);
                            hero.AddSkillXp(skillForWeapon, MBRandom.RoundRandomized(newXpAmount * xpGainMultiplier));
                        }
                    }
                    // Otherwise, add the xp as usual
                    hero.AddSkillXp(skillForWeapon, MBRandom.RoundRandomized(xpAmount * xpGainMultiplier));
                } else {
                    // Unarmed, add XP to Athletics instead
                    hero.AddSkillXp(DefaultSkills.Athletics, MBRandom.RoundRandomized(xpAmount * xpGainMultiplier));
                }

                // XP for fighting on foot
                if (!affectorMounted)
                {
                    float num1 = 0.2f;
                    if ((double)movementSpeedDamageModifier > 0.0)
                        num1 += 1.5f * movementSpeedDamageModifier;

                    if ((double)num1 > 0.0)
                        hero.AddSkillXp(DefaultSkills.Athletics, MBRandom.RoundRandomized((num1 * xpAmount) * xpGainMultiplier));
                }

                // TODO: Reduce reward depending on difficulty?
                // Campaign.Current.Models.DifficultyModel.GetDamageToPlayerMultiplier()
            }
        }
    }
}
