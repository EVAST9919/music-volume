using System;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.Extensions
{
    public static class VolumeExtensions
    {
        public static float ScaleForDepth(float depth, float cameraZ) => -cameraZ / Math.Max(1f, depth - cameraZ);

        public static Vector2 ToPlayfieldPosition(float scale, Vector2 positionAtZeroDepth, Vector3 camera) => (positionAtZeroDepth - camera.Xy) * scale + camera.Xy;
    }
}


