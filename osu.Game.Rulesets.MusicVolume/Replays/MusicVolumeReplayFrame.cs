using osu.Game.Rulesets.Replays;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.Replays
{
    public class MusicVolumeReplayFrame : ReplayFrame
    {
        public Vector2 Position;

        public MusicVolumeReplayFrame()
        {
        }

        public MusicVolumeReplayFrame(double time, Vector2 position)
            : base(time)
        {
            Position = position;
        }
    }
}


