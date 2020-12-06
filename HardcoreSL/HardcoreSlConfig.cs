using Exiled.API.Interfaces;
using System.ComponentModel;

namespace HardcoreSL
{
    public class HardcoreSlConfig : IConfig
    {
        [Description("Enables or Disabled HardcoreSL")]
        public bool IsEnabled { get; set; } = true;

        // Limb Damage 
        [Description("Enables the Limb Damage system.")]
        public bool EnableLimbDamage { get; set; } = true;
        [Description("The % chance that damaging the leg will cause its effect.")]
        public float LimbDamageLegEffectChance { get; set; } = 10f;
        [Description("The message to show when the leg limb damage effect is applied.")]
        public string LimbDamageLegMessage { get; set; } = "You have suffered a major leg injury and cannot walk as fast";
        [Description("The message to show when the torso limb damage effect is applied.")]
        public string LimbDamageTorsoMessage { get; set; } = "You have been shot in a vital position and are losing blood.";

        [Description("The % chance that damaging the torso will cause its effect.")]
        public float LimbDamageTorsoEffectChance { get; set; } = 10f;
        [Description("The % chance that damaging the head will cause its effect.")]
        public float LimbDamageHeadEffectChance { get; set; } = 100f;
        [Description("Damage that occurs on the Torso's bleed.")]
        public float TorsoBleedAmount { get; set; } = 1f;
        [Description("Delay between bleed on the Torso's bleed damage.")]
        public float TorsoBleedDelay { get; set; } = 1f;

        // Blink Time
        [Description("Enables the Reduced Blink system.")]
        public bool EnableReducedBlink { get; set; } = true;
        [Description("The scale to multiply the blink delays by.")]
        public float BlinkDelayScale { get; set; } = 0.25f;

        // 939 Bleed
        [Description("Enables the SCP-939 bleed system.")]
        public bool Enable939BiteBleed { get; set; } = true;
        [Description("How much players bit by SCP-939 will bleed for each tick.")]
        public float Scp939BleedAmount { get; set; } = 1f;
        [Description("Time between bleed damage on players bit by SCP-939.")]
        public float Scp939BleedDelay { get; set; } = 1f;

        // Logicer Damage increase
        [Description("Enables the Logicer Damage Multiplier Reversion system.")]
        public bool EnableFullLogicerDamage { get; set; } = true;

        // Light Blacking Out
        [Description("Enables the Delayed Blackout.")]
        public bool EnableBlackout { get; set; } = true;

        [Description("How long until the next blackout begins.")]
        public float BlackoutDuration { get; set; } = 60f;

        // Chaos Message
        [Description("Chaos-only Message.")]
        public string ChaosOnlyMessage { get; set; } = "Chaos Insurgency Forces and SCP Subjects solely remain. Finish this.";
    }
}
