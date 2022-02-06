using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public static class Extensions
    {
        /// <summary>
        /// acc�de � la valeur gr�ce � un vector2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Get<T>(this T[,] matrix, Vector2 value)
        {
            return matrix[(int)value.x, (int)value.y];
        }

        /// <summary>
        /// Retourne un �l�ment al�atoire d'une liste
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }


        /// <summary>
        /// Permet de comparer un ensemble de tags et de renvoyer true si au moins un match
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static bool CompareTags(this GameObject gameObject, params string[] tags)
        {
            foreach (var tag in tags)
            {
                if (gameObject.CompareTag(tag))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Permet de cloner un ScriptableObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scriptable"></param>
        /// <returns></returns>
        public static T Clone<T>(this T scriptable) where T : ScriptableObject
        {
            var clone = Object.Instantiate(scriptable);
            clone.name = scriptable.name;
            return clone;
        }
    }
}
