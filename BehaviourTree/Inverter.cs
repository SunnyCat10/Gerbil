using Godot;
using Gerbil.BehaviourTree;
using Godot.Collections;

/// <summary>
/// Inverter decorator inverts the output of its child.
/// </summary>
public class Inverter : BaseNode
{
	BaseNode child;

    public override void _Ready()
    {
        child = (BaseNode)GetChild(0);
    }

    public override State Tick(Node2D actor, Dictionary blackboard)
    {
        State respone = child.Tick(actor, blackboard);
        if (respone == State.Succeeded)
            return State.Failed;
        else if (respone == State.Failed)
            return State.Succeeded;
        else
            return State.Running;
    }  
}