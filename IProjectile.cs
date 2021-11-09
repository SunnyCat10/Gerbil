using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Gerbil
{
    interface IProjectile
    {
        public void Fire(int damage);
        public void SelfDestruct();
        public void DisableCollision();
        public void OnBodyEntered(Node body);   
    }
}
