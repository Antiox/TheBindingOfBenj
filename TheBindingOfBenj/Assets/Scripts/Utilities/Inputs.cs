using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public static class Inputs
    {
        public static Vector2 MousePosition { get; private set; }
        public static Vector2 PlayerDirection { get; private set; }
        public static bool Attack { get; private set; }


        public static void Start()
        {
            EventManager.Instance.AddListener<OnMouseMoved>(MouseMoved);
            EventManager.Instance.AddListener<OnPlayerMoved>(PlayerMoved);
            EventManager.Instance.AddListener<OnPlayerAttacked>(PlayerAttacked);
        }


        private static void MouseMoved(OnMouseMoved e)
        {
            MousePosition = e.Position;
        }

        private static void PlayerMoved(OnPlayerMoved e)
        {
            PlayerDirection = e.Direction;
        }

        private static void PlayerAttacked(OnPlayerAttacked e)
        {
            Attack = e.Attack;
        }
    }
}
