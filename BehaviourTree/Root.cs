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

	//public Node2D Entity { get; protected set; }
	//private Timer timer;
	//private Godot.Collections.Array childArray;

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


	//public override void _Ready()
	//{
	//	Entity = (Node2D)GetParent();
	//	childArray = GetChildren();
	//	SetupChildren();
	//	SetUpTimer();
	//	timer.Connect("timeout", this, nameof(Tick));
	//	GD.Print(Entity.Name.ToString());
	//}

	//private async void SetUpTimer()
	//{
	//	timer = new Timer();
	//	timer.WaitTime = 2f;
	//	Entity.CallDeferred("add_child", timer);
	//	await ToSignal(GetTree(), "idle_frame");
	//	timer.Start();
	//}

	//private void Tick()
	//{
	//	GD.Print(Entity.Name.ToString());
	//	foreach(Node node in childArray)
	//	{
	//		if (node is ICondition)
	//		{
	//			ICondition condition = (ICondition)node;
	//			GD.Print(condition.Check());
	//		}
	//	}
	//}

	//private async void SetupChildren()
	//{
	//	foreach (Node node in childArray)
	//	{
	//		if (node is ICondition)
	//		{
	//			ICondition condition = (ICondition)node;
	//			await Task.Run(() => { condition.Setup(Entity); });
	//		}
	//	}
	//}
}
