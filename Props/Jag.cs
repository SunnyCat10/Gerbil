using Godot;
using System;
using Gerbil.Props;
using Gerbil;

public class Jag : StaticBody2D, IBreakable
{
	private const string CoinScenePath = "res://Consumables/Quicoin.tscn";
	private const string SpritePath = "/Sprite";
	private const string ParticlePath = "/CPUParticles2D";
	private const string CollisionShapePath = "/CollisionShape2D";
	
	private const int MinCoinDrop = 1;
	private const int MaxCoinDrop = 3;

	private Sprite sprite;
	private CPUParticles2D cpuParticles2D;
	private CollisionShape2D collisionShape2D;
	private PackedScene coinScene;

	public override void _Ready()
	{
		sprite = GetNode<Sprite>(GetPath() + SpritePath);
		cpuParticles2D = GetNode<CPUParticles2D>(GetPath() + ParticlePath);
		collisionShape2D = GetNode<CollisionShape2D>(GetPath() + CollisionShapePath);
		coinScene = ResourceLoader.Load<PackedScene>(CoinScenePath); //TODO: Make a system to pool drops scenes later;
	}

	public async void Break()
	{
		sprite.Visible = false;
		collisionShape2D.SetDeferred("disabled", true);
		SpawnCoins();
		cpuParticles2D.Emitting = true;
		await ToSignal(GetTree().CreateTimer(cpuParticles2D.Lifetime), "timeout");
		QueueFree();
	}

	private async void SpawnCoins()
	{
		int coinDropAmount = RandomnessManager.RandomNumberGenerator.RandiRange(MinCoinDrop, MaxCoinDrop);
		for (int i = 0; i < coinDropAmount; i++)
		{
			Quicoin quicoin = (Quicoin)coinScene.Instance();
			quicoin.GlobalPosition = GlobalPosition;
			//GetParent().AddChild(quicoin); //TODO: It might be possible to stack the addChild calls to array and ait only once for the next idle_frame!
			GetParent().CallDeferred("add_child", quicoin);
			await ToSignal(GetTree(), "idle_frame");
			quicoin.Lunch();	
		}
	}
}
