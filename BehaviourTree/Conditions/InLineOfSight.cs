using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Conditions
{
	public class InLineOfSight : BaseNode
	{
		[Export]
		private string targetKey = "target";
		//private string raycastNodePath = "/raycast";

		private bool isRunning = false;
		private (Node2D actor, Node2D target) raycastParameters;
		
		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			if (isRunning)
				return State.Running;
			Node2D target = (Node2D)blackboard[targetKey];
			raycastParameters = (actor, target);
			return State.Failed;
		}

		public override void _PhysicsProcess(float delta)
		{
			if (isRunning)
			{
				Physics2DDirectSpaceState spaceState = raycastParameters.actor.GetWorld2d().DirectSpaceState;
				var result = spaceState.IntersectRay(
					raycastParameters.actor.GlobalPosition,
					raycastParameters.target.GlobalPosition,
					new Array { this },
					((KinematicBody2D)raycastParameters.actor).CollisionMask);
			}
		}
	}
}
