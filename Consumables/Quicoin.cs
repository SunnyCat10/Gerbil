using Godot;
using System;
using Gerbil;
using Gerbil.Consumables;

public class Quicoin : RigidBody2D, IConsumable
{
	private const string CollisionShapePath = "/CollisionShape2D";
	private const int CoinMinSpeed = 8;
	private const int CoinMaxSpeed = 12;
	private const int CoinSpeedModifier = 10;
	private const float MinRotation = 0f;
	private const float MaxRotation = 3.14f;
	private const float MinFlyingTime = 1f;
	private const float MaxFlyingTime = 10f;
	private const float FlyingTimeModifier = 0.1f;

	private float randomSpeed;
	private float randomRotation;
	private float randomFlyingTime;
	private CollisionShape2D collisionShape2D;

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
		randomSpeed = CoinSpeedModifier * RandomnessManager.RandomNumberGenerator.RandiRange(CoinMinSpeed, CoinMaxSpeed);
		randomRotation = -RandomnessManager.RandomNumberGenerator.RandfRange(MinRotation, MaxRotation); // - To turn the directions up instead of down.
		randomFlyingTime = FlyingTimeModifier * RandomnessManager.RandomNumberGenerator.RandfRange(MinFlyingTime, MaxFlyingTime);
		collisionShape2D = GetNode<CollisionShape2D>(GetPath() + CollisionShapePath);
	}

	public void Lunch()
	{
		ApplyImpulse(new Vector2(), new Vector2(randomSpeed, 0).Rotated(randomRotation));
		StopFalling();
	}

	private async void StopFalling()
	{
		await ToSignal(GetTree().CreateTimer(randomFlyingTime), "timeout");
		Sleeping = true;
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		collisionShape2D.SetDeferred("disabled", false);
		GravityScale = 0f;
	}

	public void InRange(Player player)
	{
		Sleeping = false;
		ApplyImpulse(new Vector2(), (player.GlobalPosition - GlobalPosition) * 10);
	}

	public void OnCollect(Player player)
	{
		Visible = false;
		collisionShape2D.SetDeferred("disabled", true);
		QueueFree();
	}

	private void OnBodyEntered(Node body)
	{
		if(body is Player)
		{
			Player player = (Player)body;
			OnCollect(player);
		}
	}
}
