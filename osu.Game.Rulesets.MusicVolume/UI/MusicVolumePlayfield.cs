using osu.Game.Rulesets.UI;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.MusicVolume.Configuration;
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
        private static readonly Vector3 default_camera = new(BASE_SIZE * 0.5f, BASE_SIZE * 0.5f, -100);

        [Cached]
        private Bindable<Vector3> camera { get; set; } = new(default_camera);

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

        private Bindable<bool> parallaxBindable;
        private bool parallax;
        private bool defaultCameraSet;

        [BackgroundDependencyLoader]
        private void load(MusicVolumeRulesetConfigManager config)
        {
            parallaxBindable = config.GetBindable<bool>(MusicVolumeRulesetSetting.EnableParallax);

            RegisterPool<MusicVolumeCircle, DrawableMusicVolumeCircle>(300, 500);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            parallaxBindable.BindValueChanged(p => parallax = p.NewValue, true);
        }

        protected override void Update()
        {
            base.Update();

            if (parallax)
            {
                Vector2 cursorOffset = ToLocalSpace(GetContainingInputManager().CurrentState.Mouse.Position) - DrawSize * 0.5f;
                camera.Value = new Vector3(BASE_SIZE * 0.5f - cursorOffset.X * 0.05f, BASE_SIZE * 0.5f - cursorOffset.Y * 0.05f, -100);
                defaultCameraSet = false;
                return;
            }

            if (defaultCameraSet)
                return;

            camera.Value = default_camera;
            defaultCameraSet = true;
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
