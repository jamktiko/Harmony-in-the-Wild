using UnityEngine;

namespace InsaneScatterbrain.Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int RotateAround(this Vector2Int v, Vector2 rotateAround, float angle)
        {
            var vFloat = ((Vector2) v).RotateAround(rotateAround, -angle);

            return new Vector2Int(
                Mathf.RoundToInt(vFloat.x),
                Mathf.RoundToInt(vFloat.y));
        }

        public static int ManhattanDistance(this Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        public static float ManhattanDistance(this Vector2Int a, Vector2 b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}