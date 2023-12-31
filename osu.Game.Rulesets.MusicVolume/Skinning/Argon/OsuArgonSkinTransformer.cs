// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.MusicVolume.Skinning.Argon
{
    public class OsuArgonSkinTransformer : SkinTransformer
    {
        public OsuArgonSkinTransformer(ISkin skin)
            : base(skin)
        {
        }

        public override Drawable? GetDrawableComponent(ISkinComponentLookup lookup)
        {
            switch (lookup)
            {
                case OsuSkinComponentLookup osuComponent:
                    // TODO: Once everything is finalised, consider throwing UnsupportedSkinComponentException on missing entries.
                    switch (osuComponent.Component)
                    {
                        case OsuSkinComponents.Cursor:
                            return new ArgonCursor();

                        case OsuSkinComponents.CursorTrail:
                            return new ArgonCursorTrail();
                    }

                    break;
            }

            return base.GetDrawableComponent(lookup);
        }
    }
}