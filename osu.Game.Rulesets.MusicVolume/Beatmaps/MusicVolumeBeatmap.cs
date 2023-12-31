using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.MusicVolume.Objects;

namespace osu.Game.Rulesets.MusicVolume.Beatmaps
{
    public class MusicVolumeBeatmap : Beatmap<MusicVolumeHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            var totalCount = HitObjects.Count();

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Circles Count",
                    Content = totalCount.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles)
                }
            };
        }
    }
}
