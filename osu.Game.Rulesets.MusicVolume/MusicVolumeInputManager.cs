using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.MusicVolume
{
    public partial class MusicVolumeInputManager : RulesetInputManager<MusicVolumeAction>
    {
        public MusicVolumeInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum MusicVolumeAction
    {
    }
}
