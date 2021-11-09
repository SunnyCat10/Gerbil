using Godot;
using System;

public class SpikeTrap : Area2D
{
	private const string AnimatedSpritePath = "/AnimatedSprite";
	private const string PlayerPath = "/root/Map/Player";
	private const string EmptyAnimationName = "Empty";
	private const string LoadingAnimationName = "Loading";
	private const string UpAnimationName = "Up";
	private const string DownAnimationName = "Down";

	private bool isDetectingCollision = false;
	private bool isPlayerInside = false;
	private bool canDamage = true;
	private bool cycleEnded = true;
	private AnimatedSprite animatedSprite;
	private Player player;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>(GetPath() + AnimatedSpritePath);
		player = (Player)GetNode<KinematicBody2D>(PlayerPath);
		Connect("body_entered", this, nameof(OnBodyEnter));
		Connect("body_exited", this ,nameof(OnBodyExit));
	}

	public override void _Process(float delta)
	{
		if (isPlayerInside && isDetectingCollision)
		{
			OnPlayerInside();
		}
		if (cycleEnded)
		{
			cycleEnded = false;
			Disabled();
		}
	}

	private async void Disabled()
	{
		animatedSprite.Play(EmptyAnimationName);
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
		Warning();
	}
	
	private async void Warning()
	{
		animatedSprite.Play(LoadingAnimationName);
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		animatedSprite.Stop();
		Activated();
	}

	private async void Activated()
	{
		isDetectingCollision = true;
		animatedSprite.Play(UpAnimationName);	
		await ToSignal(animatedSprite, "animation_finished");
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
		animatedSprite.Play(DownAnimationName);
		await ToSignal(animatedSprite, "animation_finished");
		isDetectingCollision = false;
		//timer.Start();
		cycleEnded = true;
	}

	private void OnBodyEnter(Node body)
	{
		if (body is Player)
		{
			isPlayerInside = true;
		}
	}

	private void OnBodyExit(Node body)
	{
		if (body is Player)
		{
			isPlayerInside = false;
		}
	}

	private async void OnPlayerInside()
	{
		if (canDamage)
		{
			canDamage = false;
			player.Damage(1);
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
			canDamage = true;
		}
	}

	private void StartTrapCycle()
	{
		Disabled();
	}
}
