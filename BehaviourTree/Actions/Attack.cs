using Gerbil.BehaviourTree.Interfaces;
using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Actions
{
	public class Attack : BaseNode
	{
		[Export]
		private int AttackNumber = 1;

		[Export]
		private string targetKey = "target";

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			if (actor is IActor)
			{
				Node2D target = (Node2D)blackboard[targetKey];
				IActor actorInterface = (IActor)actor;
				actorInterface.Attack(AttackNumber, target.GlobalPosition - actor.GlobalPosition);
				return State.Succeeded;
			}
			else
				return State.Failed;
		}

		public override void _Ready()
		{

		}

	}
}
