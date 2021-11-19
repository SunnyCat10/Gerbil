using Godot;
using System;
using Gerbil.Consumables;

public class ConsumableDetector : Area2D
{
	Player player;
	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEnter));
		player = (Player)GetParent();
	}

	private void OnBodyEnter(Node body)
	{
		if (body is IConsumable)
		{
			IConsumable consumable = (IConsumable)body;
			consumable.InRange(player);
		}
	}

}
