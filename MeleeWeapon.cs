using System;
using System.Collections.Generic;
using System.Text;

namespace Gerbil
{
    public abstract class MeleeWeapon : Weapon
    {
        public float AttackCircleRadius { get; protected set; }
      
        public float CollisionArc { get; protected set; }
    }
}
