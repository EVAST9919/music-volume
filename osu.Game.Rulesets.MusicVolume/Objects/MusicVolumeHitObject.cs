using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Legacy;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.Objects
{
    public abstract class MusicVolumeHitObject : HitObject, IHasPosition, IHasComboInformation
    {
        /// <summary>
        /// Scoring distance with a speed-adjusted beat length of 1 second (ie. the speed slider balls move through their track).
        /// </summary>
        internal const float BASE_SCORING_DISTANCE = 100;

        /// <summary>
        /// Minimum preempt time at AR=10.
        /// </summary>
        public const double PREEMPT_MIN = 1800;

        /// <summary>
        /// Median preempt time at AR=5.
        /// </summary>
        public const double PREEMPT_MID = 4800;

        /// <summary>
        /// Maximum preempt time at AR=0.
        /// </summary>
        public const double PREEMPT_MAX = 7200;

        public double TimePreempt = 600;
        public double TimeFadeIn = 100;

        private HitObjectProperty<Vector2> position;

        public Bindable<Vector2> PositionBindable => position.Bindable;

        public virtual Vector2 Position
        {
            get => position.Value;
            set => position.Value = value;
        }

        public float X => Position.X;
        public float Y => Position.Y;

        private HitObjectProperty<float> scale = new HitObjectProperty<float>(1);

        public Bindable<float> ScaleBindable => scale.Bindable;

        public float Scale
        {
            get => scale.Value;
            set => scale.Value = value;
        }

        public virtual bool NewCombo { get; set; }

        private HitObjectProperty<int> comboOffset;

        public Bindable<int> ComboOffsetBindable => comboOffset.Bindable;

        public int ComboOffset
        {
            get => comboOffset.Value;
            set => comboOffset.Value = value;
        }

        private HitObjectProperty<int> indexInCurrentCombo;

        public Bindable<int> IndexInCurrentComboBindable => indexInCurrentCombo.Bindable;

        public virtual int IndexInCurrentCombo
        {
            get => indexInCurrentCombo.Value;
            set => indexInCurrentCombo.Value = value;
        }

        private HitObjectProperty<int> comboIndex;

        public Bindable<int> ComboIndexBindable => comboIndex.Bindable;

        public virtual int ComboIndex
        {
            get => comboIndex.Value;
            set => comboIndex.Value = value;
        }

        private HitObjectProperty<int> comboIndexWithOffsets;

        public Bindable<int> ComboIndexWithOffsetsBindable => comboIndexWithOffsets.Bindable;

        public int ComboIndexWithOffsets
        {
            get => comboIndexWithOffsets.Value;
            set => comboIndexWithOffsets.Value = value;
        }

        private HitObjectProperty<bool> lastInCombo;

        public Bindable<bool> LastInComboBindable => lastInCombo.Bindable;

        public bool LastInCombo
        {
            get => lastInCombo.Value;
            set => lastInCombo.Value = value;
        }

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)IBeatmapDifficultyInfo.DifficultyRange(difficulty.ApproachRate, PREEMPT_MAX, PREEMPT_MID, PREEMPT_MIN);

            Scale = LegacyRulesetExtensions.CalculateScaleFromCircleSize(difficulty.CircleSize, true);
        }

        public void UpdateComboInformation(IHasComboInformation? lastObj)
        {
            // Note that this implementation is shared with the osu!catch ruleset's implementation.
            // If a change is made here, CatchHitObject.cs should also be updated.
            ComboIndex = lastObj?.ComboIndex ?? 0;
            ComboIndexWithOffsets = lastObj?.ComboIndexWithOffsets ?? 0;
            IndexInCurrentCombo = (lastObj?.IndexInCurrentCombo + 1) ?? 0;

            // At decode time, the first hitobject in the beatmap and the first hitobject after a spinner are both enforced to be a new combo,
            // but this isn't directly enforced by the editor so the extra checks against the last hitobject are duplicated here.
            if (NewCombo || lastObj == null)
            {
                IndexInCurrentCombo = 0;
                ComboIndex++;
                ComboIndexWithOffsets += ComboOffset + 1;

                if (lastObj != null)
                    lastObj.LastInCombo = true;
            }
        }

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
