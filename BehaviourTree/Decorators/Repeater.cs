using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Decorators
{
	/// <summary>
	/// Repeat the child node behavior by set amount of times.
	/// If the child node fails, returns failed state.
	/// </summary>
	public class Repeater : BaseNode
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
			State childState = child.Tick(actor, blackboard);
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
}
