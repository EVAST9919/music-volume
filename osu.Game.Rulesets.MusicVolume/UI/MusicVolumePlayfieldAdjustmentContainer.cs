using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.UI
{
    public partial class MusicVolumePlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        private const float playfield_size_adjust = 0.7f;

        public MusicVolumePlayfieldAdjustmentContainer()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(playfield_size_adjust);

            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                FillAspectRatio = 1f,
                Child = content = new ScalingContainer { RelativeSizeAxes = Axes.Both }
            };
        }

        private partial class ScalingContainer : Container
        {
            protected override void Update()
            {
                base.Update();
                Scale = new Vector2(Parent.ChildSize.X / MusicVolumePlayfield.BASE_SIZE);
                Size = Vector2.Divide(Vector2.One, Scale);
            }
        }
    }
}
