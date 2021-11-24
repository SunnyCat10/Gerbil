using Godot;
using System;
using Gerbil.BehaviourTree;
using Godot.Collections;

/// <summary>
/// Checks if target is in range.
/// </summary>
public class InRange : BaseNode
{
	[Export]
	private string targetKey = "target";
	[Export]
	private float detectionRange = 100f;

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		Node2D target = (Node2D)blackboard[targetKey];
		if (actor.GlobalPosition.DistanceTo(target.GlobalPosition) <= detectionRange)
			return State.Succeeded;
		else
			return State.Failed;
	}
}
