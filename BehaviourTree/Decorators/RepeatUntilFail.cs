using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Decorators
{
	/// <summary>
	/// Continue to reprocess the child until the child finally returns failure,
	/// at which point this node will return succeeded state.
	/// </summary>
	public class RepeatUntilFail : BaseNode
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
					return State.Failed;
				case State.Succeeded:
					return State.Running;
			}
			return State.Failed;
		}
	}
}
