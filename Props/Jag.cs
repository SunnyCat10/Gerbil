using Godot;
using System;
using Gerbil.Props;

public class Jag : StaticBody2D, IBreakable
{
	private const string SpritePath = "/Sprite";
	private const string ParticlePath = "/CPUParticles2D";
	private const string CollisionShapePath = "/CollisionShape2D";

	private Sprite sprite;
	private CPUParticles2D cpuParticles2D;
	private CollisionShape2D collisionShape2D;

	public override void _Ready()
	{
		sprite = GetNode<Sprite>(GetPath() + SpritePath);
		cpuParticles2D = GetNode<CPUParticles2D>(GetPath() + ParticlePath);
		collisionShape2D = GetNode<CollisionShape2D>(GetPath() + CollisionShapePath);
	}

	public async void Break()
	{
		sprite.Visible = false;
		collisionShape2D.SetDeferred("disabled", true);
		cpuParticles2D.Emitting = true;
		await ToSignal(GetTree().CreateTimer(cpuParticles2D.Lifetime), "timeout");
		QueueFree();
	}
}
