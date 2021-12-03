using Godot;
using System;
using Gerbil.Enemies;
using Gerbil;
using Gerbil.BehaviourTree.Interfaces;

public class Warrior : Enemy, IEnemy, IActor
{
	private const int Damage = 1;
	private PackedScene projectileScene;
	private const string ProjectileScenePath = "res://Projectiles/EnemyProjectile.tscn";

	private const string CollisionShapePath = "/CollisionShape2D";
	private const string AnimatedSpritePath = "/AnimatedSprite";
	private const string DeathAnimationName = "";
	private const int MaxHealth = 100;

	private bool IsCharging = false;
	private Vector2 chargeStartPosition;
	private Vector2 chargeEndPosition;

	public override void _Ready()
	{
		Health = MaxHealth;
		collisionShape = GetNode<CollisionShape2D>(GetPath() + CollisionShapePath);
		animatedSprite = GetNode<AnimatedSprite>(GetPath() + AnimatedSpritePath);
		deathAnimationName = DeathAnimationName;

		projectileScene = ResourceLoader.Load<PackedScene>(ProjectileScenePath);
	}

    public override void _PhysicsProcess(float delta)
    {
		if (IsCharging)
        {

        }
    }

    public void Shoot(Vector2 shootingDirection)
	{
		RigidBody2D projectile = (RigidBody2D)projectileScene.Instance();
		projectile.GlobalPosition = GlobalPosition;
		projectile.Rotation = shootingDirection.Angle();
		GetParent().AddChild(projectile);
		IProjectile projectileInterface = (IProjectile)projectile;
		projectileInterface.Fire(Damage);
	}

	public void Attack(int attackNumber, Vector2 attackingDirection)
	{
		switch (attackNumber)
		{
			case 1:
				ChargedAttack(attackingDirection);
				break;
			default:
				break;
		}
	}

	private async void ChargedAttack(Vector2 attackingDirection)
    {
		IsCharging = true;
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		IsCharging = false;
	}
}
