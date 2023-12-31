// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.MusicVolume.Configuration;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.UI.Cursor
{
    public partial class OsuCursorContainer : GameplayCursorContainer
    {
        public new OsuCursor ActiveCursor => (OsuCursor)base.ActiveCursor;

        protected override Drawable CreateCursor() => new OsuCursor();

        protected override Container<Drawable> Content => fadeContainer;

        private readonly Container<Drawable> fadeContainer;

        private readonly Bindable<bool> showTrail = new Bindable<bool>(true);

        private readonly Drawable cursorTrail;

        public OsuCursorContainer()
        {
            InternalChild = fadeContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new[]
                {
                    cursorTrail = new SkinnableDrawable(new OsuSkinComponentLookup(OsuSkinComponents.CursorTrail), _ => new DefaultCursorTrail(), confineMode: ConfineMode.NoScaling),
                }
            };
        }

        [BackgroundDependencyLoader(true)]
        private void load(MusicVolumeRulesetConfigManager rulesetConfig)
        {
            rulesetConfig?.BindWith(MusicVolumeRulesetSetting.ShowCursorTrail, showTrail);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            showTrail.BindValueChanged(v => cursorTrail.FadeTo(v.NewValue ? 1 : 0, 200), true);

            ActiveCursor.CursorScale.BindValueChanged(e =>
            {
                var newScale = new Vector2(e.NewValue);
                cursorTrail.Scale = newScale;
            }, true);
        }

        public override bool HandlePositionalInput => true; // OverlayContainer will set this false when we go hidden, but we always want to receive input.

        protected override void PopIn()
        {
            fadeContainer.FadeTo(1, 300, Easing.OutQuint);
            ActiveCursor.ScaleTo(1f, 400, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            fadeContainer.FadeTo(0.05f, 450, Easing.OutQuint);
            ActiveCursor.ScaleTo(0.8f, 450, Easing.OutQuint);
        }

        private partial class DefaultCursorTrail : CursorTrail
        {
            [BackgroundDependencyLoader]
            private void load(TextureStore textures)
            {
                Texture = textures.Get(@"Cursor/cursortrail");
                Scale = new Vector2(1 / Texture.ScaleAdjust);
            }
        }
    }
}
