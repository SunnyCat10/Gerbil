using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Decorators
{
	/// <summary>
	/// Inverts the output of the node. thus succeeded turns to failed and via versa.
	/// </summary>
	public class Inverter : BaseNode
	{
		private BaseNode child;

		public override void _Ready()
		{
			child = (BaseNode)GetChild(0);
		}

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			State state = child.Tick(actor, blackboard);
			switch (state)
			{
				case State.Running:
					return State.Running;
				case State.Failed:
					return State.Succeeded;
				case State.Succeeded:
					return State.Failed;
			}
			return State.Failed;
		}      
	}
}
