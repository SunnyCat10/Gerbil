using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using System.Threading.Tasks;

namespace Gerbil.BehaviourTree
{
    interface ICondition
    {
        public void Setup(Node2D instance);

        public bool Check();
    }
}
