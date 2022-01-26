using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerCombat : Entity
    {
        private InputManager _inputManager;

        protected override void Awake()
        {
            base.Awake();
            _inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        }

        protected override void Update()
        {
            base.Update();
            var attack = _inputManager.Attack;

            if (attack)
            {
                Attack(_inputManager.MousePosition);
            }
        }
    }
}
