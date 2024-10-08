﻿using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using osu.Framework.Input;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.MusicVolume.Objects;
using osu.Game.Rulesets.MusicVolume.Replays;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.UI
{
    public partial class DrawableMusicVolumeRuleset : DrawableRuleset<MusicVolumeHitObject>
    {
        public DrawableMusicVolumeRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true; // always show the gameplay cursor

        protected override PassThroughInputManager CreateInputManager() => new MusicVolumeInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => new MusicVolumePlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new MusicVolumePlayfieldAdjustmentContainer();

        public override DrawableHitObject<MusicVolumeHitObject> CreateDrawableRepresentation(MusicVolumeHitObject h) => null;

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new MusicVolumeFramedReplayInputHandler(replay);
    }
}
