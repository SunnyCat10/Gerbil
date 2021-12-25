using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.ControlFlows
{
	public class Parallel : BaseNode
	{
		[Export]
		private Policy policy = Policy.OneSucceeded;

		private Array children;
		private State[] results;
		private delegate State ExecutionPolicy(Node2D actor, Dictionary blackboard);
		private ExecutionPolicy executionPolicy;
		private enum Policy
		{
			OneSucceeded,
			AllSucceeded,
			AllFinishedRunning
		}

		public override void _Ready()
		{
			children = GetChildren();
			results = new State[children.Count];
            switch (policy)
            {
                case Policy.OneSucceeded:
                    executionPolicy = OneSucceededPolicy;
					break;
                case Policy.AllSucceeded:
                    break;
                case Policy.AllFinishedRunning:
                    break;
                default:
					executionPolicy = OneSucceededPolicy;
					break;
            }
        }

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			return executionPolicy(actor, blackboard);
		}

		//TODO: Implement
		private State OneSucceededPolicy(Node2D actor, Dictionary blackboard)
        {
			GD.Print("Not Implemented Yet");
			for (int i = 0; i < children.Count; i++)
			{
				if (results[i] == State.Running)
					results[i] = ((BaseNode)children[i]).Tick(actor, blackboard);

				if (results[i] == State.Succeeded)
                {
					for (int j = 0; j < children.Count; j++) // Resets all the states of the children.
						children[j] = State.Running;
					return State.Succeeded;
                }
			}
			return State.Failed;
		}

		//TODO: Implement
		private State AllSucceededPolicy(Node2D actor, Dictionary blackboard)
        {
			GD.Print("Not Implemented Yet");
			return State.Failed;
        }

		//TODO: Implement
		private State AllFinishedRunningPolicy(Node2D actor, Dictionary blackboard)
        {
			GD.Print("Not Implemented Yet");
			return State.Failed;
		}
	}
}
