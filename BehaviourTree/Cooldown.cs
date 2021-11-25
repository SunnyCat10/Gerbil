using Godot;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class Cooldown : BaseNode
{
	[Export]
	float duration = 3f;

	bool isInCooldown = false;

	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		if (isInCooldown)
			return State.Running;
		GetTree().CreateTimer(duration).Connect("timeout", this, nameof(Timeout));
		isInCooldown = true;
		return State.Succeeded;
	}

	public void Timeout()
	{
		isInCooldown = false;
	} 
}
