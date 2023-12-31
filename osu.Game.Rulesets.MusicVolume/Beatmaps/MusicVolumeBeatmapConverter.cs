using System;
using osu.Game.Beatmaps;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Objects;
using System.Threading;
using osuTK;
using osu.Game.Rulesets.MusicVolume.Objects;
using osu.Game.Rulesets.MusicVolume.UI;

namespace osu.Game.Rulesets.MusicVolume.Beatmaps
{
    public class MusicVolumeBeatmapConverter : BeatmapConverter<MusicVolumeHitObject>
    {
        public MusicVolumeBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        protected override IEnumerable<MusicVolumeHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;

            switch (original)
            {
                case IHasPathWithRepeats curve:
                    yield return new MusicVolumeCircle
                    {
                        StartTime = original.StartTime,
                        Samples = original.Samples,
                        Position = toMusicVolumePlayfield(positionData?.Position ?? Vector2.Zero),
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                    };

                    yield return new MusicVolumeCircle
                    {
                        StartTime = original.StartTime + curve.Duration,
                        Samples = original.Samples,
                        Position = toMusicVolumePlayfield(positionData?.Position + curve.CurvePositionAt(curve.Duration) ?? Vector2.Zero),
                        NewCombo = false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                    };

                    break;

                default:
                    yield return new MusicVolumeCircle
                    {
                        StartTime = original.StartTime,
                        Samples = original.Samples,
                        Position = toMusicVolumePlayfield(positionData?.Position ?? Vector2.Zero),
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                    };

                    break;
            }
        }

        protected override Beatmap<MusicVolumeHitObject> CreateBeatmap() => new MusicVolumeBeatmap();

        private static Vector2 toMusicVolumePlayfield(Vector2 osuPosition)
        {
            float relativeX = Math.Clamp(osuPosition.X, 0, 512) / 512;
            float relativeY = Math.Clamp(osuPosition.Y, 0, 384) / 384;
            float x = relativeX * (MusicVolumePlayfield.BASE_SIZE * 0.8f) + MusicVolumePlayfield.BASE_SIZE * 0.1f;
            float y = relativeY * (MusicVolumePlayfield.BASE_SIZE * 0.8f) + MusicVolumePlayfield.BASE_SIZE * 0.1f;
            return new Vector2(x, y);
        }
    }
}
