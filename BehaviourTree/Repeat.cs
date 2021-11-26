using Godot;
using Gerbil.BehaviourTree;
using Godot.Collections;

/// <summary>
/// Repeat the child node behavior by set amount.
/// </summary>
public class Repeat : BaseNode
{
	[Export]
	private int amount = 3;

	private int counter = 0;
	private BaseNode child;

	public override void _Ready()
	{	
		child = (BaseNode)GetChild(0);
	}

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		return TickChild(actor, blackboard);
	}

	private State TickChild(Node2D actor, Dictionary blackboard)
	{
		State childState;
		childState = child.Tick(actor, blackboard);
		switch (childState)
		{
			case State.Running:
				return State.Running;
			case State.Failed:
				counter = 0;
				return State.Failed;
			case State.Succeeded:
				++counter;
				if (counter >= amount)
				{
					counter = 0;
					return State.Succeeded;
				}
				return State.Running;
		}
		return State.Failed;
	}
}