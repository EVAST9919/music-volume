﻿using System.Collections.Generic;
using osu.Framework.Input.StateChanges;
using osu.Framework.Utils;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.MusicVolume.Replays
{
    public class MusicVolumeFramedReplayInputHandler : FramedReplayInputHandler<MusicVolumeReplayFrame>
    {
        public MusicVolumeFramedReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        protected override bool IsImportant(MusicVolumeReplayFrame frame) => true;

        protected override void CollectReplayInputs(List<IInput> inputs)
        {
            var position = Interpolation.ValueAt(CurrentTime, StartFrame.Position, EndFrame.Position, StartFrame.Time, EndFrame.Time);

            inputs.Add(new MousePositionAbsoluteInput { Position = GamefieldToScreenSpace(position) });
        }
    }
}


