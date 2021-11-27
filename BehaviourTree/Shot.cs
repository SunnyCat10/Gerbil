using Godot;
using Gerbil.BehaviourTree.Interfaces;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class Shot : BaseNode
{
	[Export]
	private string targetKey = "target";
	
	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		if (actor is IActor)
		{
			Node2D target = (Node2D)blackboard[targetKey];
			IActor actorInterface = (IActor)actor;
			actorInterface.Shoot(target.GlobalPosition - actor.GlobalPosition);
			return State.Succeeded;
		}
		return State.Failed;
	}
}
