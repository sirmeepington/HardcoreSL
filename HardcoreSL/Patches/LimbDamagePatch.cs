using HarmonyLib;
using UnityEngine;

namespace HardcoreSL.Patches
{
    
    [HarmonyPatch(typeof(WeaponManager),nameof(WeaponManager.CallCmdShoot))]
    public class LimbDamagePatch
    { 
        private static void Postfix(WeaponManager __instance, HitBoxType hitboxType, GameObject target)
        {
            // If limb damage is disabled; just don't do anything on the patch.
            if (!HardcoreSl.Instance.Config.EnableLimbDamage)
                return;

            if (target == null || __instance.gameObject == null)
                return;

            if (!ReferenceHub.TryGetHub(target, out var targetHub) || !ReferenceHub.TryGetHub(__instance.gameObject, out var ourHub))
                return;

            // Copied basically from HurtPlayer. Prevent limb damage on friendly fire targets.
            bool ff = targetHub.characterClassManager.Fraction == ourHub.characterClassManager.Fraction;
            if (ff && !GameCore.ConfigFile.ServerConfig.GetBool("friendly_fire"))
            {
                return;
            }

            if (!__instance.TryGetComponent(out PlayerHardcoreComponent instanceComponent) ||
                !target.TryGetComponent(out PlayerHardcoreComponent targetComponent))
                return;

            // No limb damage on SCPs - with the exception of 049-2.
            if (targetHub.characterClassManager.IsAnyScp() && targetHub.characterClassManager.CurClass != RoleType.Scp0492)
                return;

            float dis = Vector3.Distance(__instance.camera.transform.position, target.transform.position);
            float damage = __instance.weapons[__instance.curWeapon].damageOverDistance.Evaluate(dis);

            targetComponent.RegisterHit(hitboxType,damage,instanceComponent);
        }

    }
}
