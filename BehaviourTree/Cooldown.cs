using Godot;
using Gerbil.BehaviourTree;
using Godot.Collections;

/// <summary>
/// Cooldown node for creating delay between actions.
/// </summary>
public class Cooldown : BaseNode
{
	[Export]
	float duration = 3f;

	[Export]
	private bool onStart = true;

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

		//if (onStart)
		//{
		//	SetupTimer();
		//	onStart = false;
		//}
		//if (isInCooldown)
		//	return State.Running;
		//State childState = child.Tick(actor, blackboard);
		//if (childState == State.Succeeded)
		//{
		//	SetupTimer();
		//	return State.Succeeded; 
		//}
		//else
		//	return childState;
	}

	public void Timeout()
	{
		isInCooldown = false;
	} 

	private void SetupTimer()
	{
		GetTree().CreateTimer(duration).Connect("timeout", this, nameof(Timeout));
		isInCooldown = true;
	}
}
