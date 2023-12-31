using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.MusicVolume.Judgements
{
    public class MusicVolumeJudgement : Judgement
    {
        public override HitResult MaxResult => HitResult.Perfect;
    }
}
