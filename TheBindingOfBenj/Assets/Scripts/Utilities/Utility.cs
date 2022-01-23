using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public static class Utility
    {
        public static Vector2 RotateVector2(Vector2 v, float angle)
        {
            var rad = Mathf.Deg2Rad * angle;
            return new Vector2(
                v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
                v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad));
        }
        public static Vector2 RotateVector2(float angle)
        {
            var rad = Mathf.Deg2Rad * angle;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
        public static Vector3 RotateVector3(float angle)
        {
            var rad = Mathf.Deg2Rad * angle;
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
}
