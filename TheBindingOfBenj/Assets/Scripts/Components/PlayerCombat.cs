using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerCombat : Entity
    {
        protected override void Update()
        {
            base.Update();
            if (Inputs.Attack)
            {
                Attack(Inputs.MousePosition);
            }
        }
    }
}
