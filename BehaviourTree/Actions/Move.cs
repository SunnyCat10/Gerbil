using Gerbil.BehaviourTree.Interfaces;
using Godot;
using Godot.Collections;

/// <summary>
/// Calls IActor movement function.
/// </summary>
namespace Gerbil.BehaviourTree.Actions
{
	public class Move : BaseNode
	{
		[Export]
		private string targetKey = "target";

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			if (actor is IActor)
			{
				Node2D target = (Node2D)blackboard[targetKey];
				IActor actorInterface = (IActor)actor;
				actorInterface.Move(target.GlobalPosition, true); //new Vector2(100,-100) ,true);//target.GlobalPosition, true);
				return State.Succeeded;
			}
			else
				return State.Failed;
		}
	}
}
