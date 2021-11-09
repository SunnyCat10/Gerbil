using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gerbil.Weapons
{
    interface IMelee : IWeapon
    {
        public Task Attack();
        public void ChargedAttack();
    }
}
