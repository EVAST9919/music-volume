using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.MusicVolume.Configuration
{
    public class MusicVolumeRulesetConfigManager : RulesetConfigManager<MusicVolumeRulesetSetting>
    {
        public MusicVolumeRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetValue(MusicVolumeRulesetSetting.ShowCursorTrail, true);
        }
    }

    public enum MusicVolumeRulesetSetting
    {
        ShowCursorTrail
    }
}


