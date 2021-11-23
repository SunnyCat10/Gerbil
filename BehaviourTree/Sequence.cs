using Godot;
using System;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class Sequence : BaseNode
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
			if (response != State.Succeeded)
				return response;
		}
		return State.Succeeded;
	}
}
