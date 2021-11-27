using Godot;
using System;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class Sequence : BaseNode
{
	private Godot.Collections.Array children;
	private bool isRunning = false;

	public override void _Ready()
	{
		children = GetChildren();
	}

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		if (isRunning)
			return State.Running;
		isRunning = true;
		foreach (BaseNode baseNode in children)
		{
			State response = baseNode.Tick(actor, blackboard);
			if (response != State.Succeeded)
			{
				isRunning = false;
				return response;
			}
		}
		isRunning = false;
		return State.Succeeded;
	}
}
