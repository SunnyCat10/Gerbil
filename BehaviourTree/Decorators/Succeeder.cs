using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Decorators
{
	/// <summary>
	/// Always return success. Useful when processing a branch of a tree where a failure is expected,
	/// but it should not abandon the processing of a sequence that the branch sits on.
	/// </summary>
	public class Succeeder : BaseNode
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
					return State.Succeeded;
			}
			return State.Succeeded;
		}
	}
}
