using JetBrains.Annotations;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.MusicVolume.Objects.Drawables
{
    public abstract partial class DrawableMusicVolumeHitObject<T> : DrawableHitObject<MusicVolumeHitObject>
        where T : MusicVolumeHitObject
    {
        protected new T HitObject => (T)base.HitObject;

        protected DrawableMusicVolumeHitObject([CanBeNull] MusicVolumeHitObject h = null)
            : base(h)
        {
        }
    }
}
