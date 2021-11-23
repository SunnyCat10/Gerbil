using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree
{
    public abstract class BaseNode : Node
    {
        public enum State
        {
            Running,
            Failed,
            Succeeded
        }
        public abstract State Tick(Node2D actor, Dictionary blackboard);
    }
}
