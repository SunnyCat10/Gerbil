using System;
using System.Collections.Generic;
using System.Text;

namespace Gerbil.Consumables
{
    interface IConsumable
    {
        public void InRange(Player player);
        public void OnCollect(Player player);
    }
}
