using Exiled.API.Features;
using HarmonyLib;
using RemoteAdmin;
using UnityEngine;

namespace HardcoreSL.Patches
{
    [HarmonyPatch(typeof(Scp939PlayerScript),nameof(Scp939PlayerScript.CallCmdShoot))]
    public class Scp939BleedPatch
    {

        private static void Postfix(Scp939PlayerScript __instance, GameObject target)
        {

            if (!HardcoreSl.Instance.Config.Enable939BiteBleed)
                return;

            if (target == null || __instance.gameObject == null)
                return;

            PlayerHardcoreComponent instanceComponent = __instance.GetComponent<PlayerHardcoreComponent>();
            PlayerHardcoreComponent targetComponent = target.GetComponent<PlayerHardcoreComponent>();
            if (instanceComponent == null || targetComponent == null)
                return;

            targetComponent.Bleed(HardcoreSl.Instance.Config.Scp939BleedAmount, HardcoreSl.Instance.Config.Scp939BleedDelay);
        }
    }
}
