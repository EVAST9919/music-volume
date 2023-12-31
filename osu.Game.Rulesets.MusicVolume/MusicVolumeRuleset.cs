using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using System;
//using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.MusicVolume.Beatmaps;
using osu.Game.Rulesets.MusicVolume.Configuration;
//using osu.Framework.Allocation;
//using osu.Framework.Platform;
using osu.Game.Rulesets.MusicVolume.Difficulty;
using osu.Game.Rulesets.MusicVolume.Mods;
using osu.Game.Rulesets.MusicVolume.Skinning.Argon;
using osu.Game.Rulesets.MusicVolume.Skinning.Legacy;
using osu.Game.Rulesets.MusicVolume.UI;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.MusicVolume
{
    public partial class MusicVolumeRuleset : Ruleset
    {
        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableMusicVolumeRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new MusicVolumeBeatmapConverter(beatmap, this);

        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new BeatmapProcessor(beatmap);

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new MusicVolumeRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new MusicVolumeSettingsSubsection(this);

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => Array.Empty<KeyBinding>();

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new MusicVolumeModNoFail(),
                        new MultiMod(new MusicVolumeModHalfTime(), new MusicVolumeModDaycore())
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new MusicVolumeModSuddenDeath(),
                        new MultiMod(new MusicVolumeModDoubleTime(), new MusicVolumeModNightcore())
                    };

                /*case ModType.Automation:
                    return new Mod[]
                    {
                        new BosuModAutoplay()
                    };*/

                case ModType.Fun:
                    return new Mod[]
                    {
                        new MultiMod(new ModWindUp(), new ModWindDown())
                    };

                default:
                    return Array.Empty<Mod>();
            }
        }

        public override string Description => "Music Volume";

        public override string ShortName => "mv";

        public override string PlayingVerb => "Feeling the music";

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Regular.Clone
        };

        protected override IEnumerable<HitResult> GetValidHitResults() => new[]
        {
            HitResult.Perfect
        };

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new MusicVolumeDifficultyCalculator(RulesetInfo, beatmap);

        public override ISkin? CreateSkinTransformer(ISkin skin, IBeatmap beatmap)
        {
            switch (skin)
            {
                case LegacySkin:
                    return new OsuLegacySkinTransformer(skin);

                case ArgonSkin:
                    return new OsuArgonSkinTransformer(skin);
            }

            return null;
        }

        /*private partial class MusicVolumeIcon : Sprite
        {
            private readonly MusicVolumeRuleset ruleset;

            public MusicVolumeIcon(MusicVolumeRuleset ruleset)
            {
                this.ruleset = ruleset;
            }

            [BackgroundDependencyLoader]
            private void load(GameHost host)
            {
                Texture = new TextureStore(host.Renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/logo");
            }
        }*/
    }
}
