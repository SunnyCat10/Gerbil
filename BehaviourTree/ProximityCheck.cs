using Godot;
using System;
using Gerbil.BehaviourTree;
using System.Threading.Tasks;
using Godot.Collections;

public class ProximityCheck : BaseNode
{
	//[Export]
	//private float range = 0f;
	//[Export]
	//private Node2D target;

	//private Node2D instance;

	//public override void _Ready()
	//{

	//}

	//public bool Check()
	//{
	//    if (target == null)
	//        return false;
	//    else
	//        return instance.GlobalPosition.DistanceTo(target.GlobalPosition) <= range;
	//}

	//public async void Setup(Node2D instance)
	//{
	//    await ToSignal(this, "ready");
	//    this.instance = instance;
	//}
	public override State Tick(Node2D actor, Dictionary blackboard)
	{
		//GD.Print(actor.Name, blackboard["delta"]);
		return State.Succeeded;
	}
}
