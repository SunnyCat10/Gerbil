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
		//return TickChild(actor, blackboard);	
		//return Tick2(actor, blackboard);
		return Tick3(actor, blackboard);
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

	private State Tick2(Node2D actor, Dictionary blackboard)
	{
		for (int i = 0; i < amount; i++)
		{	
			State childState = child.Tick(actor, blackboard);
			GD.Print("RAFT");
			if (childState == State.Failed)
				return State.Failed;
		}
		GD.Print("Done");
		return State.Succeeded;
	}

	private State Tick3(Node2D actor, Dictionary blackboard)
	{
		State childState = child.Tick(actor,blackboard);
		switch (childState)
		{
			case State.Running:
				return State.Running;
			case State.Failed:
				counter = 0;
				return State.Failed;
			case State.Succeeded:
				++counter;
				if (counter == amount)
				{
					counter = 0;
					return State.Succeeded;
				}
				else
					return State.Running;
		}
		return State.Failed;
	}
}
