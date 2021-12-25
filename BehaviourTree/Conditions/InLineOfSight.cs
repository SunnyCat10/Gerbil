using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Conditions
{
	/// <summary>
	/// Checks if a target node is in direct line of sight of the parent.
	/// Based on physical raycast query so it can detect objects that obsecure the line of sight.
	/// </summary>
	public class InLineOfSight : BaseNode
	{
		[Export]
		private string targetKey = "target";

		/// <summary>
		/// Enum to differentiate the raycast query result.
		/// </summary>
		private enum RayCastResult
		{
			Empty,
			Running,
			TargetHit,
			TargetNotHit
		}

		/// <summary>
		/// Stores raycast query parameters so the physics process can access them.
		/// </summary>
		private (Node2D actor, Node2D target) raycastParameters; 
		private RayCastResult rayCastResult = RayCastResult.Empty;

		private World2D world;
		
		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			if (rayCastResult == RayCastResult.Empty) // Ready to do the raycast query.
			{
				Node2D target = (Node2D)blackboard[targetKey];
				raycastParameters = (actor, target);
				rayCastResult = RayCastResult.Running;
				return State.Running;
			}
			else if (rayCastResult == RayCastResult.Running) // Query is currently running.
				return State.Running;
			else // Return the results of the raycast query and default it`s state.
			{
				State state = State.Failed;
				switch (rayCastResult)
				{
					case RayCastResult.TargetHit:
						state = State.Succeeded;
						break;
					case RayCastResult.TargetNotHit:
						state = State.Failed;
						break;
				}
				rayCastResult = RayCastResult.Empty;
				return state;
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			if (rayCastResult == RayCastResult.Running)
			{
				world = raycastParameters.actor.GetWorld2d();
				Physics2DDirectSpaceState spaceState = world.DirectSpaceState;
				Dictionary result = spaceState.IntersectRay(
					raycastParameters.actor.GlobalPosition,
					raycastParameters.target.GlobalPosition,
					new Array { raycastParameters.actor },
					((KinematicBody2D)raycastParameters.actor).CollisionMask);
				if (result.Count > 0)
				{
					if ((int)result["collider_id"] == (int)(raycastParameters.target.GetInstanceId()))
						rayCastResult = RayCastResult.TargetHit;
					else
						rayCastResult = RayCastResult.TargetNotHit;
				}
				else
					rayCastResult = RayCastResult.TargetNotHit;
			}			
		}

		// WARNING: cleanup: ObjectDB Instances still exist! when using GetWorld2d().DirectSpaceState #39216
		// TODO: this is a Workaround! fix it when Godot devs will have a fix.
		public override void _ExitTree()
		{
			world?.Dispose();
		}
	}
}
