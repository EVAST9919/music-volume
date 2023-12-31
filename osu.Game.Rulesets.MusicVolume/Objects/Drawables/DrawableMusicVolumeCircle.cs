using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.MusicVolume.UI;

namespace osu.Game.Rulesets.MusicVolume.Objects.Drawables
{
    public partial class DrawableMusicVolumeCircle : DrawableMusicVolumeHitObject<MusicVolumeCircle>
    {
        private static readonly Vector3 camera_position = new Vector3(MusicVolumePlayfield.BASE_SIZE * 0.5f, MusicVolumePlayfield.BASE_SIZE * 0.5f, -100);

        public readonly IBindable<Vector2> PositionBindable = new Bindable<Vector2>();

        public DrawableMusicVolumeCircle()
            : this(null)
        {
        }

        public DrawableMusicVolumeCircle([CanBeNull] MusicVolumeCircle h = null)
            : base(h)
        {
        }

        private Container container;

        [BackgroundDependencyLoader]
        private void load()
        {
            Alpha = 0;

            Origin = Anchor.Centre;
            Size = new Vector2(MusicVolumePlayfield.BASE_SIZE * 0.2f);
            AddInternal(container = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderThickness = 15,
                CornerRadius = 20,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                }
            });

            //PositionBindable.BindValueChanged(p => Position = p.NewValue);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AccentColour.BindValueChanged(c => container.BorderColour = c.NewValue, true);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(HitObject.TimeFadeIn);
        }

        protected override void Update()
        {
            base.Update();

            if (HitObject == null)
                return;

            double speed = 800 / HitObject.TimePreempt;
            double appearTime = HitObject.StartTime - HitObject.TimePreempt;
            float z = 800 - (float)((Math.Max(Time.Current, appearTime) - appearTime) * speed);

            computeProperties(z);
        }

        protected override bool OnHover(HoverEvent e) => true;

        private void computeProperties(float z)
        {
            z = Math.Max(z, 0);

            float scale = scaleForDepth(z);
            Position = toPlayfieldPosition(scale, PositionBindable.Value);
            Scale = new Vector2(scale);
        }

        private static float scaleForDepth(float depth) => -camera_position.Z / Math.Max(1f, depth - camera_position.Z);

        private static Vector2 toPlayfieldPosition(float scale, Vector2 positionAtZeroDepth)
        {
            return (positionAtZeroDepth - camera_position.Xy) * scale + camera_position.Xy;
        }

        public override bool HandlePositionalInput => true;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset < 0)
                return;

            Vector2 mousePos = ToLocalSpace(GetContainingInputManager().CurrentState.Mouse.Position);

            if (mousePos.X >= 0 && mousePos.X <= Size.X && mousePos.Y >= 0 && mousePos.Y <= Size.Y)
            {
                ApplyResult(h => h.Type = h.Judgement.MaxResult);
                return;
            }

            ApplyResult(r => r.Type = r.Judgement.MinResult);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            base.UpdateHitStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                case ArmedState.Hit:
                    this.FadeOut(100);
                    break;
            }
        }

        protected override void OnApply()
        {
            base.OnApply();

            PositionBindable.BindTo(HitObject.PositionBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();

            PositionBindable.UnbindFrom(HitObject.PositionBindable);
        }

        protected override double InitialLifetimeOffset => HitObject.TimePreempt;
    }
}
