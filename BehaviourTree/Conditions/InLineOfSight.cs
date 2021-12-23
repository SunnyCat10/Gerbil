using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Conditions
{
	public class InLineOfSight : BaseNode
	{
		private enum RayCastResult
		{
			Empty,
			Running,
			HitPlayer,
			HitSomethingElse,
			HitNothing
		}

		[Export]
		private string targetKey = "target";
		//private string raycastNodePath = "/raycast";

		private bool isRunning = false;
		private (Node2D actor, Node2D target) raycastParameters;
		private RayCastResult rayCastResult = RayCastResult.Empty;


		private World2D world;
		
		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			if (rayCastResult == RayCastResult.Empty)
			{
				GD.Print("Started");
				Node2D target = (Node2D)blackboard[targetKey];
				raycastParameters = (actor, target);
				rayCastResult = RayCastResult.Running;
				return State.Running;
			}
			else if (rayCastResult == RayCastResult.Running)
				return State.Running;
			else
			{
				State state = State.Failed;
				switch (rayCastResult)
				{
					case RayCastResult.HitPlayer:
						state = State.Succeeded;
						break;
					case RayCastResult.HitSomethingElse:
						state = State.Failed;
						break;
					case RayCastResult.HitNothing:
						state = State.Failed;
						break;
				}
				rayCastResult = RayCastResult.Empty;
				isRunning = false;
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
						GD.Print("Hit");
				}
				else
					rayCastResult = RayCastResult.HitNothing;
			}			
		}

		// WARNING: cleanup: ObjectDB Instances still exist! when using GetWorld2d().DirectSpaceState #39216
		// TODO:Workaround
		public override void _ExitTree()
		{
			world?.Dispose();
		}
	}
}
