using System;
using osuTK;

namespace osu.Game.Rulesets.MusicVolume.Extensions
{
    public static class MathExtensions
    {
        public static Vector2 Rotate(Vector2 origin, float length, float angle)
        {
            double rad = angle * Math.PI / 180.0;
            return origin + new Vector2((float)Math.Cos(rad), (float)Math.Sin(rad)) * length;
        }

        public static Vector2 RotateAround(Vector2 origin, Vector2 point, float angle)
        {
            float rad = (float)(angle * Math.PI / 180.0);
            float s = MathF.Sin(rad);
            float c = MathF.Cos(rad);

            point -= origin;

            return new Vector2(point.X * c - point.Y * s, point.X * s + point.Y * c) + origin;
        }

        public static float GetAngle(Vector2 origin, Vector2 pos)
        {
            double a = Math.Atan2(origin.Y - pos.Y, origin.X - pos.X) - Math.PI * 0.5;
            if (a < 0.0)
                a += Math.PI * 2;

            a /= Math.PI * 2;
            a *= 360;
            return (float)a;
        }
    }
}
