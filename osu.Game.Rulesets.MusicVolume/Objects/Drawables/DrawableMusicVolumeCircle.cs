using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.MusicVolume.Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.MusicVolume.UI;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MusicVolume.Objects.Drawables
{
    public partial class DrawableMusicVolumeCircle : DrawableMusicVolumeHitObject<MusicVolumeCircle>
    {
        public readonly IBindable<Vector2> PositionBindable = new Bindable<Vector2>();

        [Resolved]
        private Bindable<Vector3> cameraPosition { get; set; }

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
                EdgeEffect = new EdgeEffectParameters
                {
                    Radius = 10,
                    Colour = Color4.Black.Opacity(0.2f),
                    Type = EdgeEffectType.Shadow,
                    Hollow = true
                },
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AccentColour.BindValueChanged(c => container.BorderColour = c.NewValue, true);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeColour(Color4.White);
            this.FadeInFromZero(HitObject.TimePreempt * 0.2f);
        }

        private const float max_depth = 1000;

        protected override void Update()
        {
            base.Update();

            if (HitObject == null)
                return;

            if (Result.HasResult && !Result.IsHit)
                return;

            double speed = max_depth / HitObject.TimePreempt;
            double appearTime = HitObject.StartTime - HitObject.TimePreempt;
            float z = max_depth - (float)((Math.Max(Time.Current, appearTime) - appearTime) * speed);

            computeProperties(z);
        }

        private void computeProperties(float z)
        {
            Vector3 cameraPos = cameraPosition.Value;
            float scale = VolumeExtensions.ScaleForDepth(z, cameraPos.Z);
            Position = VolumeExtensions.ToPlayfieldPosition(scale, PositionBindable.Value + cameraPos.Xy - new Vector2(MusicVolumePlayfield.BASE_SIZE * 0.5f), cameraPos);
            Scale = new Vector2(scale);
        }

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
                    this.FadeOut(200);
                    this.RotateTo(25, 200, Easing.InOutQuad);
                    this.FadeColour(Color4.Red, 200, Easing.Out);
                    break;

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
