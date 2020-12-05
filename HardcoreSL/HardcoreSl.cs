using Exiled.API.Features;
using HarmonyLib;
using MEC;
using System;

namespace HardcoreSL
{
    public class HardcoreSl : Plugin<HardcoreSlConfig>
    {

        public override string Author => "SirMeepington";

        public override string Name => "Hardcore SCP:SL";

        public override Version Version => new Version(0, 6, 9);

        private EventHandler _events;

        public CoroutineHandle LightHandle;

        /// <summary>
        /// Singleton instance of the plugin.
        /// </summary>
        public static HardcoreSl Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            Harmony harmony = new Harmony("sirmeepington.hardcoresl");
            harmony.PatchAll();

            RegisterEvents();

            // :)
            Log.Info("Hardcore SCP:SL by SirMeep");
            Log.Info("      /| ________________");
            Log.Info("O|===|* >________________>");
            Log.Info("      \\|");
            Log.Info($"Running Version v{Version}");

        }

        private void RegisterEvents()
        {
            _events = new EventHandler(this);
            Exiled.Events.Handlers.Player.Joined += _events.Joined;
            Exiled.Events.Handlers.Player.Died += _events.Died;
            Exiled.Events.Handlers.Server.EndingRound += _events.CheckRoundEnd;
            Exiled.Events.Handlers.Player.ChangingRole += _events.ChangeRole;
            Exiled.Events.Handlers.Server.RoundStarted += _events.BeginLight;
            Exiled.Events.Handlers.Server.RespawningTeam += _events.Respawn;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Joined -= _events.Joined;
            Exiled.Events.Handlers.Player.Died -= _events.Died;
            Exiled.Events.Handlers.Server.EndingRound -= _events.CheckRoundEnd;
            Exiled.Events.Handlers.Player.ChangingRole -= _events.ChangeRole;
            Exiled.Events.Handlers.Server.RoundStarted -= _events.BeginLight;
            _events = null;

            Timing.KillCoroutines(LightHandle);
        }


    }
}
