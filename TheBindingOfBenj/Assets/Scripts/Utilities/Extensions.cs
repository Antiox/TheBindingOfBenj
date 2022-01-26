using System;
using System.Collections;
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
            return matrix[(int) value.x, (int) value.y];
        }
    }
}
