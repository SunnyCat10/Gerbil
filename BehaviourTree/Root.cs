using Godot;
using System;
using System.Threading.Tasks;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class Root : Node
{
	private Dictionary blackboard;
	private BaseNode child;
	private Node2D actor;

	public override void _Ready()
	{
		blackboard = new Dictionary();
		child = (BaseNode)GetChild(0);
		actor = (Node2D)GetParent();
	}

	public override void _Process(float delta)
	{
		if (blackboard.Contains("delta"))
			blackboard["delta"] = delta;
		else
			blackboard.Add("delta", delta);
		child.Tick(actor, blackboard);
	}
}
