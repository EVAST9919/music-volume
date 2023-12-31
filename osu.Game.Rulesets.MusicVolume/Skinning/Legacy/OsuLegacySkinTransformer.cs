// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.MusicVolume.Skinning.Legacy
{
    public class OsuLegacySkinTransformer : LegacySkinTransformer
    {
        public override bool IsProvidingLegacyResources => base.IsProvidingLegacyResources || hasHitCircle.Value;

        private readonly Lazy<bool> hasHitCircle;

        public OsuLegacySkinTransformer(ISkin skin)
            : base(skin)
        {
            hasHitCircle = new Lazy<bool>(() => GetTexture("hitcircle") != null);
        }

        public override Drawable? GetDrawableComponent(ISkinComponentLookup lookup)
        {
            if (lookup is OsuSkinComponentLookup osuComponent)
            {
                switch (osuComponent.Component)
                {
                    case OsuSkinComponents.Cursor:
                        if (GetTexture("cursor") != null)
                            return new LegacyCursor(this);

                        return null;

                    case OsuSkinComponents.CursorTrail:
                        if (GetTexture("cursortrail") != null)
                            return new LegacyCursorTrail(this);

                        return null;

                    default:
                        throw new UnsupportedSkinComponentException(lookup);
                }
            }

            return base.GetDrawableComponent(lookup);
        }
    }
}