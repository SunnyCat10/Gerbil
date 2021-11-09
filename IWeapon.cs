using System;
using System.Collections.Generic;
using System.Text;
using Gerbil;
using Godot;

namespace Gerbil
{
    interface IWeapon
    {
        public Weapon OnPickUp(Node2D picker);
    }
}
