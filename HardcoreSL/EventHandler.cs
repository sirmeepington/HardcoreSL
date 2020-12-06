using Exiled.API.Features;
using Exiled.Events.EventArgs;
using HardcoreSL.Util;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HardcoreSL
{
    public class EventHandler
    {
        private readonly HardcoreSl _plugin;

        private bool respawnHappened = true;

        public EventHandler(HardcoreSl plugin)
        {
            _plugin = plugin;
        }

        internal void Joined(JoinedEventArgs ev)
        {
            ev.Player.GameObject.AddComponent<PlayerHardcoreComponent>();
        }

        internal void Died(DiedEventArgs ev)
        {
            CheckRemaining();
        }

        private void CheckRemaining()
        {
            List<Player> alive = Player.List.Where(x => x.IsAlive).ToList();
            bool anyOtherAlive = alive.All(x => x.Team == Team.SCP || x.Team == Team.CHI);
            bool allScps = alive.All(x => x.Team == Team.SCP);
            int amountAlive = alive.Count(x => x.Team != Team.SCP);
            if (allScps)
                return;
            if (!anyOtherAlive)
                return;
            if (!respawnHappened)
                return;
            Server.Broadcast.RpcAddElement(_plugin.Config.ChaosOnlyMessage, 10, Broadcast.BroadcastFlags.Normal);
            respawnHappened = false;
        }

        internal void ChangeRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent(out PlayerHardcoreComponent comp))
            {
                comp.Bleeding = false; // Stop bleeding if they change role via RA, escape, etc. (includes death)
            }
        }

        internal void Respawn(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam != Respawning.SpawnableTeamType.ChaosInsurgency)
                respawnHappened = true;
        }

        internal void BeginLight()
        {
            if (!_plugin.Config.EnableBlackout)
                return;
            _plugin.LightHandle = Timing.RunCoroutine(FlickLights());
        }

        private IEnumerator<float> FlickLights()
        {
            while (this != null)
            {
                yield return Timing.WaitForSeconds(_plugin.Config.BlackoutDuration);
                List<Room> rooms = Map.Rooms.ToList();
                rooms.ShuffleListSecure();
                foreach (Room room in rooms.Take(5))
                {
                    room.TurnOffLights(_plugin.Config.BlackoutDuration);
                }
            }
        }

        internal void CheckRoundEnd(EndingRoundEventArgs ev)
        {
            if (Player.List.Any(v => v.Team == Team.CHI) && Player.List.Any(b => b.Team == Team.SCP))
                ev.IsAllowed = false;
        }

    }
}
