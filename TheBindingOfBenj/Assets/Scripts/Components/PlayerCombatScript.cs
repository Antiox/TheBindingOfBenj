using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerCombatScript : Entity
    {
        protected override void Update()
        {
            base.Update();
            if (Inputs.Attack)
            {
                Attack(Inputs.MousePosition);
            }
        }

        public void ChangeWeapon(Weapon weapon)
        {
            _equipedWeapon = weapon;
        }

    }
}
