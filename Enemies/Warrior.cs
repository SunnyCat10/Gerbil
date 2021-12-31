using Godot;
using Gerbil.Enemies;
using Gerbil;
using Gerbil.BehaviourTree.Interfaces;
using System.Collections.Generic;
using Gerbil.Utilities;

public class Warrior : Enemy, IEnemy, IActor
{
	private const int Damage = 1;
	private PackedScene projectileScene;
	private const string ProjectileScenePath = "res://Projectiles/EnemyProjectile.tscn";
	private const string PathfindingPath = "/Pathfinding";

	private const string CollisionShapePath = "/CollisionShape2D";
	private const string AnimatedSpritePath = "/AnimatedSprite";
	private const string DeathAnimationName = "";
	private const int MaxHealth = 100;

	private const string DeathAnimation = "Death";
	private const string MovingAnimation = "Moving";
	private const string ShootingAnimation = "Shooting";


	private bool IsCharging = false;
	private Vector2 movementDirection;
	private bool IsMoving = false;
	private Pathfinding pathfinding;

	private Vector2 targetLocation;
	private Vector2[] path = new Vector2[0];
	private List<Vector2> debugPathList;

	private bool enabled = true;

	public override void _Ready()
	{
		Health = MaxHealth;
		collisionShape = GetNode<CollisionShape2D>(GetPath() + CollisionShapePath);
		animatedSprite = GetNode<AnimatedSprite>(GetPath() + AnimatedSpritePath);
		pathfinding = (Pathfinding)GetNode<Node2D>(GetParent().GetPath() + PathfindingPath);
		deathAnimationName = DeathAnimationName;

		projectileScene = ResourceLoader.Load<PackedScene>(ProjectileScenePath);

		animatedSprite = GetNode<AnimatedSprite>(GetPath() + AnimatedSpritePath);

		debugPathList = new List<Vector2>();

		Health = MaxHealth;
		deathAnimationName = DeathAnimation;

		animatedSprite.Play(MovingAnimation);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (enabled)
		{
			//if (IsCharging)
			//	MoveAndSlide(movementDirection * delta * 10000f * 2f);
			if (IsMoving)
			{
				//Vector2[] path = pathfinding.GetNewPath(GlobalPosition, targetLocation);
				if (path.Length > 1)
				{
					MoveAndCollide((path[1] - GlobalPosition).Normalized() * delta * 100f);

					debugPathList = new List<Vector2>();
					foreach (Vector2 point in path)
					{
						debugPathList.Add(point);
					}
					Debug.Instance.AddPathFindingPath(Name, debugPathList);
				}
			}
		}
	}

	public async void Shoot(Vector2 shootingDirection)
	{
		if (enabled)
		{
			animatedSprite.Play(ShootingAnimation);
			await ToSignal(animatedSprite, "animation_finished");
			animatedSprite.Play(MovingAnimation);
			RigidBody2D projectile = (RigidBody2D)projectileScene.Instance();
			projectile.GlobalPosition = GlobalPosition;
			projectile.Rotation = shootingDirection.Angle();
			GetParent().AddChild(projectile);
			IProjectile projectileInterface = (IProjectile)projectile;
			projectileInterface.Fire(Damage);
		}
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
		movementDirection = attackingDirection;
		IsCharging = true;
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		IsCharging = false;
	}
	 
	public void Move(Vector2 targetLocation, bool isRunning)
	{
		if (enabled)
		{
			IsMoving = isRunning;
			this.targetLocation = targetLocation;
			path = pathfinding.GetNewPath(GlobalPosition, targetLocation);
		}
	}

	public override async void OnDeath()
	{	
		collisionShape.SetDeferred("disabled", true);
		enabled = false;
		animatedSprite.Play(deathAnimationName);
		await ToSignal(animatedSprite, "animation_finished");
		QueueFree();
	}
}
