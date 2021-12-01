using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Selector
{
	/// <summary>
	/// Processing the children in order, if one of them succeeded return success. if all failed return failed.
	/// Analagous with an OR gate.
	/// </summary>
	public class Selector : BaseNode
	{
		private Godot.Collections.Array children;
		private int currentRunningChild = 0;

		public override void _Ready()
		{
			children = GetChildren();
		}

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			BaseNode child = (BaseNode)children[currentRunningChild];
			State response = child.Tick(actor, blackboard);
			switch (response)
			{
				case State.Running:
					return State.Running;
				case State.Failed:
					++currentRunningChild;
					if (currentRunningChild == children.Count)
					{
						currentRunningChild = 0;
						return State.Failed;
					}
					else
						return State.Running;
				case State.Succeeded:
					currentRunningChild = 0;
					return State.Succeeded;
			}
			return State.Failed;
		}		
	}
}