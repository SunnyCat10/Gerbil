using Godot;
using System;
using Gerbil.BehaviourTree;
using Godot.Collections;

public class WarriorBehaviour : Node
{
	private const string TargetKey = "target";
	private const string PlayerPath = "/root/Map/Player";

	private Dictionary blackboard;
	private BaseNode child;
	private Node2D actor;

	private Player player;

	public override void _Ready()
	{
		blackboard = new Dictionary();
		child = (BaseNode)GetChild(0);
		actor = (Node2D)GetParent();
		player = (Player)GetNode(PlayerPath);
	}

	public override void _Process(float delta)
	{
		UpdateBlackBoard();
		child.Tick(actor, blackboard);
	}

	private void UpdateBlackBoard()
	{
		blackboard[TargetKey] = player;
	}
}
