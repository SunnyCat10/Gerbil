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

	bool isInCooldown = false;
	BaseNode child;

	public override void _Ready()
	{
		child = (BaseNode)GetChild(0);
	}

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		if (isInCooldown)
		{
			return State.Running;
		}
		State childState = child.Tick(actor, blackboard);
		if (childState == State.Succeeded)
		{
			GetTree().CreateTimer(duration).Connect("timeout", this, nameof(Timeout));
			isInCooldown = true;
			return State.Succeeded;
		}
		else
		{
			return childState;
		}
	}

	public void Timeout()
	{
		isInCooldown = false;
	} 
}