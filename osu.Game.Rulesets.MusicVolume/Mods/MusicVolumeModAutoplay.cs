using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.MusicVolume.Replays;

namespace osu.Game.Rulesets.MusicVolume.Mods
{
    public class MusicVolumeModAutoplay : ModAutoplay
    {
        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new(new MusicVolumeAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "Autoplay" });
    }
}
