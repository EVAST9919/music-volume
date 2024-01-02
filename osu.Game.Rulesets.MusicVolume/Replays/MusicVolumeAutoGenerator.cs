using System;
using JetBrains.Annotations;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.MusicVolume.Beatmaps;
using osu.Game.Rulesets.MusicVolume.UI;
using osu.Game.Rulesets.Replays;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.Replays
{
    public class MusicVolumeAutoGenerator : AutoGenerator
    {
        public new MusicVolumeBeatmap Beatmap => (MusicVolumeBeatmap)base.Beatmap;

        public MusicVolumeAutoGenerator([NotNull] IBeatmap beatmap)
            : base(beatmap)
        {
            Replay = new Replay();
        }

        protected Replay Replay;

        public override Replay Generate()
        {
            Vector2 lastPos = new Vector2(MusicVolumePlayfield.BASE_SIZE * 0.5f);
            Replay.Frames.Add(new MusicVolumeReplayFrame(-10000, lastPos));

            double lastTime = Beatmap.HitObjects[0].StartTime - 1000;

            for (int i = 0; i < Beatmap.HitObjects.Count; i++)
            {
                var h = Beatmap.HitObjects[i];

                int frameCount = Math.Max((int)((h.StartTime - lastTime) / 16.6666), 1);

                for (int j = 0; j < frameCount; j++)
                    Replay.Frames.Add(new MusicVolumeReplayFrame(lastTime + j * 16.6666f, Interpolation.ValueAt(j, lastPos, h.Position, 0, frameCount)));

                lastTime = h.StartTime;
                lastPos = h.Position;
            }

            return Replay;
        }
    }
}


