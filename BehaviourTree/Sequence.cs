using Godot;
using System;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class Sequence : BaseNode
{
	private Godot.Collections.Array children;
	private int currentRunningChild = 0;

	public override void _Ready()
	{
		children = GetChildren();
	}

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		BaseNode child = (BaseNode)children[currentRunningChild];
		State response = child.Tick(actor, blackboard);
		switch (response)
		{
			case State.Running:
				return State.Running;
			case State.Failed:
				currentRunningChild = 0;
				return State.Failed;
			case State.Succeeded:
				++currentRunningChild;
				if (currentRunningChild == children.Count) 
				{
					currentRunningChild = 0;
					return State.Succeeded;
				}
				else
					return State.Running;
		}
		return State.Failed;

		//foreach (BaseNode baseNode in children)
		//{
		//	State response = baseNode.Tick(actor, blackboard);
		//	if (response != State.Succeeded)
		//	{
		//		return response;
		//	}
		//}
		//return State.Succeeded;
	}
}
