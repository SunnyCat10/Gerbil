using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.ControlFlows 
{ 
	/// <summary>
	/// Processing the children in order, if one of them failed return failed state. if all succeeded return succeeded state.
	/// Analagous with an AND gate.
	/// Turn on the random check mark in the editor to make the node process the children in random order.
	/// </summary>
	public class Sequence : BaseNode
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
					if (random)
						children.Shuffle();
					currentRunningChild = 0;
					return State.Failed;
				case State.Succeeded:
					++currentRunningChild;
					if (currentRunningChild == children.Count) 
					{
						if (random)
							children.Shuffle();
					currentRunningChild = 0;
					return State.Succeeded;
					}
					else
						return State.Running;
			}
			return State.Failed;
		}
	}
}