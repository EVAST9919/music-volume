using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.MusicVolume.Configuration;

namespace osu.Game.Rulesets.MusicVolume.UI
{
    public partial class MusicVolumeSettingsSubsection : RulesetSettingsSubsection
    {
        protected override LocalisableString Header => "Music Volume";

        public MusicVolumeSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (MusicVolumeRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Show Cursor Trail",
                    Current = config.GetBindable<bool>(MusicVolumeRulesetSetting.ShowCursorTrail)
                }
            };
        }
    }
}


