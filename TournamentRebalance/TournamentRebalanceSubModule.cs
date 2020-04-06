using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using SandBox;
using SandBox.TournamentMissions.Missions;
using System;
using System.Windows;
using HarmonyLib;
using System.Reflection;
using TaleWorlds.Localization;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TournamentRebalance
{
    public class TournamentRebalanceSubModule : MBSubModuleBase
    {
        // TODO: Tournament Prize List in TournamentGame

        protected override void OnGameStart(Game game, IGameStarter gameStartedObject)
        {
            if (!(game.GameType is Campaign))
                return;

            gameStartedObject.AddModel(new RebalancedTournamentModel());
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                var harmony = new Harmony("com.dealman.tournament.patch");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            } catch (Exception exception) {
                MessageBox.Show($"An Error Occurred!\n\nError Message:\n\n{exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);

            if (mission.HasMissionBehaviour<ArenaPracticeFightMissionController>())
            {
                // Note: Release of patch e1.0.6 added combat XP gains for Tournaments and Arenas gain. Thus this has been disabled but left in place for educational purposes

                //if (!mission.HasMissionBehaviour<ArenaXPController>())
                    //mission.AddMissionBehaviour(new ArenaXPController());
            } else if (mission.HasMissionBehaviour<TournamentArcheryMissionController>() || mission.HasMissionBehaviour<TournamentFightMissionController>() || mission.HasMissionBehaviour<TournamentJoustingMissionController>()) {
                //if (!mission.HasMissionBehaviour<TournamentXPController>())
                    //mission.AddMissionBehaviour(new TournamentXPController());
            }
        }
    }
}