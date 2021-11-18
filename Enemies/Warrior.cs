using Godot;
using System;
using Gerbil.Enemies;
using Gerbil;

public class Warrior : Enemy, IEnemy
{
	private const string CollisionShapePath = "/CollisionShape2D";
	private const string AnimatedSpritePath = "/AnimatedSprite";
	private const string DeathAnimationName = "";
	private const int MaxHealth = 100;

	public override void _Ready()
	{
		Health = MaxHealth;
		collisionShape = GetNode<CollisionShape2D>(GetPath() + CollisionShapePath);
		animatedSprite = GetNode<AnimatedSprite>(GetPath() + AnimatedSpritePath);
		deathAnimationName = DeathAnimationName;
	}


}
