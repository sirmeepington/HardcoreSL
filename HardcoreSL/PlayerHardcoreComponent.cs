using CustomPlayerEffects;
using Exiled.API.Features;
using HardcoreSL.Util;
using System.Linq;
using UnityEngine;

namespace HardcoreSL
{
    public class PlayerHardcoreComponent : MonoBehaviour
    {
        private ReferenceHub _hub;

        #region Bleeding Vars
        private float _bleedAmount;
        private float _bleedTime;
        private int _bleedTicks = -1;
        private bool _bleeding;
        private float _bleedDelay;
        /// <summary>
        /// Returns whether or not the current player is bleeding.
        /// <para/>
        /// The bleeding takes off health at a given <see cref="BleedDelay"/>
        /// </summary>
        public bool Bleeding 
        {
            get => _bleeding;
            set
            {
                if (!value)
                {
                    _bleedTicks = -1;
                }

                _bleeding = value;
            }
        }
        public float BleedDelay 
        {
            get => _bleedDelay;
            set 
            {
                _bleedTime = value;
                _bleedDelay = value;
            }
        }

        #endregion

        private void Start()
        {
            _hub = GetComponent<ReferenceHub>(); // Assign refhub for usage later.

            if (HardcoreSl.Instance.Config.EnableReducedBlink && 
                TryGetComponent(out Scp173PlayerScript scp173)) // Faster blinking for all players. :)
            {   
                scp173.maxBlinkTime *= HardcoreSl.Instance.Config.BlinkDelayScale;
                scp173.minBlinkTime *= HardcoreSl.Instance.Config.BlinkDelayScale;
            }

            if (HardcoreSl.Instance.Config.EnableFullLogicerDamage &&
                TryGetComponent(out WeaponManager wepManager)) // 100% damage to SCPs w/ Logicer
            {
                // I either do this fuckery or use its explicit index.
                var logicer = wepManager.weapons.FirstOrDefault(x => x.inventoryID == ItemType.GunLogicer);
                if (logicer != null)
                {
                    int index = wepManager.weapons.IndexOf(logicer);
                    wepManager.weapons[index].scpDamageMultiplier = 1; // Reset logicer's weapon damage.
                }
            }

            _hub.nicknameSync.NetworkViewRange = 0f; // Hide name plate
        }

        private void Update()
        {
            if (Bleeding)
            {
                DoBleed();
            }
        }

        #region Bleeding

        /// <summary>
        /// Makes the current player begin bleeding for the specified amount
        /// at the specified delay for the optionally specified maximum bleed
        /// ticks. 
        /// <para/>
        /// Bleeding can be stopped by setting <see cref="Bleeding"/> to false.
        /// </summary>
        /// <param name="amount">Amount of health each bleed tick will take from 
        /// the player's health..</param>
        /// <param name="delay">The delay between bleeding ticks.</param>
        /// <param name="maxTicks">The maximum amount of ticks before the bleeding
        /// stops. -1 makes the player bleed until it is prevented externally.</param>
        public void Bleed(float amount, float delay, int maxTicks = -1)
        {
            if (Bleeding) // Already bleeding.
                return;
            BleedDelay = delay;
            _bleedAmount = amount;
            _bleedTicks = maxTicks;
            Bleeding = true;
        }

        private void DoBleed()
        {
            if (_bleedTime > 0)
            {
                _bleedTime -= Time.deltaTime;
                return;
            }

            if (_bleedTicks == 0)
            {
                Bleeding = false;
                return;
            }

            bool dead = _hub.playerStats.HurtPlayer(
                new PlayerStats.HitInfo(_bleedAmount, _hub.LoggedNameFromRefHub(), DamageTypes.Bleeding, _hub.playerId),
                _hub.gameObject);

            if (dead)
            {
                Bleeding = false;
            }
            else
            {
                _bleedTime = BleedDelay;
                _bleedTicks--;
            }
        }

        #endregion


        #region Limb Damage
        public void RegisterHit(HitBoxType hitbox,  float damage, PlayerHardcoreComponent by)
        {
            if (hitbox == HitBoxType.NULL || hitbox == HitBoxType.WINDOW || hitbox == HitBoxType.HOLE)
                return;


            switch(hitbox)
            {
                case HitBoxType.HEAD:
                    DamageHead(by.gameObject.GetComponent<ReferenceHub>(), damage);
                    break;
                case HitBoxType.LEG:
                    DamageLeg();
                    break;
                case HitBoxType.BODY:
                    DamageTorso();
                    break;
            }
        }

        private void DamageHead(ReferenceHub shotBy, float damage)
        {
            // Hurt again to do double damage.
            if (!RandomUtil.Chance(HardcoreSl.Instance.Config.LimbDamageHeadEffectChance))
                return;

            shotBy.playerStats.HurtPlayer(new PlayerStats.HitInfo(damage, shotBy.LoggedNameFromRefHub(), DamageTypes.FromWeaponId(shotBy.weaponManager.curWeapon), shotBy.queryProcessor.PlayerId), _hub.gameObject);
        }

        private void DamageLeg()
        {
            if (!RandomUtil.Chance(HardcoreSl.Instance.Config.LimbDamageLegEffectChance))
            {
                return;
            }

            // Shot in leg; slowed.
            _hub.playerEffectsController.EnableEffect<Disabled>();
            _hub.hints.ShowText(HardcoreSl.Instance.Config.LimbDamageLegMessage, 4);
        }

        private void DamageTorso()
        {
            if (!RandomUtil.Chance(HardcoreSl.Instance.Config.LimbDamageTorsoEffectChance))
            {
                return;
            }

            Bleed(HardcoreSl.Instance.Config.TorsoBleedAmount, HardcoreSl.Instance.Config.TorsoBleedDelay); // Bleed for 5hp every second.
            _hub.hints.ShowText(HardcoreSl.Instance.Config.LimbDamageTorsoMessage, 4);
        }

        #endregion
    }
}
