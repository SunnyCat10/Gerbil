using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Decorators
{
	/// <summary>
	/// Timer node for creating delay between actions. the timer starts when the node recive tick signal.
	/// </summary>
	public class Timer : BaseNode
	{
		[Export]
		float duration = 3f;

		private bool readyToCountdown = true;
		private bool isInCooldown = false;
		private bool isWithoutChild = false;
		BaseNode child;

		public override void _Ready()
		{
			if (GetChildCount() == 0)
				isWithoutChild = true;
			else
				child = (BaseNode)GetChild(0);
		}

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			if (readyToCountdown)
			{
				SetupTimer();
				readyToCountdown = false;
				return State.Running;
			}
			if (isInCooldown)
				return State.Running;
			State childState = child.Tick(actor, blackboard);
			switch (childState)
			{
				case State.Running:
					return State.Running;
				case State.Failed:
					return State.Failed;
				case State.Succeeded:
					readyToCountdown = true;
					return State.Succeeded;
			}
			return State.Failed;
		}

		private void SetupTimer()
		{
			GetTree().CreateTimer(duration).Connect("timeout", this, nameof(OnTimeout));
			isInCooldown = true;
		}

        private void OnTimeout()
        {
            isInCooldown = false;
        }
    }
}
