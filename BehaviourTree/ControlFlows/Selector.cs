using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.ControlFlows
{
	/// <summary>
	/// Processing the children in order, if one of them succeeded return succeeded state. if all failed return failed state.
	/// Analagous with an OR gate.
	/// Turn on the random check mark in the editor to make the node process the children in random order.
	/// </summary>
	public class Selector : BaseNode
	{
		[Export]
		private bool random = false;

		private Array children;
		private int currentRunningChild = 0;

		public override void _Ready()
		{
			children = GetChildren();
			if (random)
				children.Shuffle();
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
						if (random)
							children.Shuffle();
						currentRunningChild = 0;
						return State.Failed;
					}
					else
						return State.Running;
				case State.Succeeded:
					if (random)
						children.Shuffle();
					currentRunningChild = 0;
					return State.Succeeded;
			}
			return State.Failed;
		}		
	}
}
