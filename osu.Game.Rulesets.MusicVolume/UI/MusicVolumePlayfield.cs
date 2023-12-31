using osu.Game.Rulesets.UI;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.MusicVolume.Objects;
using osu.Game.Rulesets.MusicVolume.Objects.Drawables;
using osu.Game.Rulesets.MusicVolume.UI.Cursor;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MusicVolume.UI
{
    public partial class MusicVolumePlayfield : Playfield
    {
        public static readonly float BASE_SIZE = 512f;

        public MusicVolumePlayfield()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderThickness = 5,
                    CornerRadius = 10,
                    BorderColour = Color4.White,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        AlwaysPresent = true,
                        Alpha = 0
                    }
                },
                HitObjectContainer
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RegisterPool<MusicVolumeCircle, DrawableMusicVolumeCircle>(300, 500);
        }

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new MusicVolumeHitObjectLifetimeEntry(hitObject);

        protected override GameplayCursorContainer CreateCursor() => new OsuCursorContainer();

        private class MusicVolumeHitObjectLifetimeEntry : HitObjectLifetimeEntry
        {
            public MusicVolumeHitObjectLifetimeEntry(HitObject hitObject)
                : base(hitObject)
            {
            }

            protected override double InitialLifetimeOffset
            {
                get
                {
                    if (HitObject is MusicVolumeCircle circle)
                        return circle.TimePreempt;

                    return base.InitialLifetimeOffset;
                }
            }
        }
    }
}
