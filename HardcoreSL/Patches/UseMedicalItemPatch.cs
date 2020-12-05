using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardcoreSL.Patches
{
    [HarmonyPatch(typeof(PlayerEffectsController),nameof(PlayerEffectsController.UseMedicalItem))]
    public class UseMedicalItemPatch
    {
        private static void Postfix(PlayerEffectsController __instance)
        {
            if (__instance.gameObject == null)
                return;

            if (!__instance.TryGetComponent(out PlayerHardcoreComponent comp))
            {
                return;
            }

            if (__instance.TryGetComponent(out PlayerEffectsController controller) && controller.GetEffect<CustomPlayerEffects.Disabled>().Enabled)
                controller.DisableEffect<CustomPlayerEffects.Disabled>();

            comp.Bleeding = false;
        }

    }
}
