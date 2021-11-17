using Godot;
using System;
using Gerbil;

public class Quicoin : RigidBody2D
{
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

	public override void _Ready()
	{
		randomSpeed = CoinSpeedModifier * RandomnessManager.RandomNumberGenerator.RandiRange(CoinMinSpeed, CoinMaxSpeed);
		randomRotation = -RandomnessManager.RandomNumberGenerator.RandfRange(MinRotation, MaxRotation); // - To turn the directions up instead of down.
		randomFlyingTime = FlyingTimeModifier * RandomnessManager.RandomNumberGenerator.RandfRange(MinFlyingTime, MaxFlyingTime);
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
	}
}
