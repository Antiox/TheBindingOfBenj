using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

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
		/// <summary>
        /// Permet de retourner true ou false en fonction de si a et b son � peu pr�s �gaux.
        /// Cette fonction peut �tre utile car m�me si a et b sont strictement �gaux, "a == b" ne renvoie pas syst�matiquement true, de fait de l'impr�cision des float.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        /// <summary>
        /// Permet de retourner sous forme de liste, l'ensemble des �l�ments d'un type Enum
        /// </summary>
        /// <typeparam name="T">L'enum que l'on veut lister</typeparam>
        /// <returns>La liste des �l�ments de l'enum</returns>
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Permet d'obtenir un vector2 avec des composantes al�atoires, selon une plage donn�e
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Vector2 GetRandomVector2(float from, float to)
        {
            var x = Random.Range(from, to);
            var y = Random.Range(from, to);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pivot"></param>
        /// <param name="angles"></param>
        /// <returns></returns>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            var dir = Quaternion.Euler(angles) * (point - pivot);
            return dir + pivot;
        }

        public static Vector2 RandomPointInAnnulus(Vector2 point, float minRadius, float maxRadius)
        {
            return point + (Random.insideUnitCircle * point).normalized * Random.Range(minRadius, maxRadius);
        }

    }
}
