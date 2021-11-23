using Godot;
using Gerbil.BehaviourTree;
using Godot.Collections;

/// <summary>
/// Executes its children by order, it stops when one of them return
/// successed or running. if a child returns failed it will try the next one.
/// If all the childs fail it will return failed state.
/// </summary>
public class Selector : BaseNode
{
	private Godot.Collections.Array children;

	public override void _Ready()
	{
		children = GetChildren();
	}

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		foreach (BaseNode baseNode in children)
		{
			State response = baseNode.Tick(actor, blackboard);
			if (response != State.Failed)
				return response;
		}
		return State.Failed;
	}
}