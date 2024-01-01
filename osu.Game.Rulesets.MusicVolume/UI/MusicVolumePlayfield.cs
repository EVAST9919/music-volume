using osu.Game.Rulesets.UI;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.MusicVolume.Extensions;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.MusicVolume.Objects;
using osu.Game.Rulesets.MusicVolume.Objects.Drawables;
using osu.Game.Rulesets.MusicVolume.UI.Cursor;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MusicVolume.UI
{
    public partial class MusicVolumePlayfield : Playfield
    {
        public static readonly float BASE_SIZE = 512f;

        [Cached]
        private Bindable<Vector3> camera { get; set; } = new(new Vector3(MusicVolumePlayfield.BASE_SIZE * 0.5f, MusicVolumePlayfield.BASE_SIZE * 0.5f, -100));

        public MusicVolumePlayfield()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            InternalChildren = new Drawable[]
            {
                HitObjectContainer,
                new MusicVolumeBorder()
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RegisterPool<MusicVolumeCircle, DrawableMusicVolumeCircle>(300, 500);
        }

        protected override void Update()
        {
            base.Update();

            Vector2 cursorOffset = ToLocalSpace(GetContainingInputManager().CurrentState.Mouse.Position) - DrawSize * 0.5f;
            camera.Value = new Vector3(MusicVolumePlayfield.BASE_SIZE * 0.5f - cursorOffset.X * 0.05f, MusicVolumePlayfield.BASE_SIZE * 0.5f - cursorOffset.Y * 0.05f, -100);
        }

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new MusicVolumeHitObjectLifetimeEntry(hitObject);

        protected override GameplayCursorContainer CreateCursor() => new OsuCursorContainer();

        private partial class MusicVolumeBorder : Container
        {
            [Resolved]
            private Bindable<Vector3> cameraPosition { get; set; }

            public MusicVolumeBorder()
            {
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Both;
                Masking = true;
                BorderThickness = 5;
                CornerRadius = 10;
                BorderColour = Color4.White;
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                };
            }

            protected override void Update()
            {
                base.Update();

                Vector3 cameraPos = cameraPosition.Value;
                Position = VolumeExtensions.ToPlayfieldPosition(1f, cameraPos.Xy, cameraPos);
            }
        }

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
