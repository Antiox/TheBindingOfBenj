using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptables/Enemy")]
    public class Enemy : ScriptableObject
    {
        public Sprite artwork;
        public float Health;
        public bool IsFlying;

        [HideInInspector]
        public IPattern pattern;
    }
}
