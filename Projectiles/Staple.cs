using Godot;
using System;
using Gerbil;

public class Staple : RigidBody2D, IProjectile
{
	private const string WEAPON_NAME = "";
	private const string COLLISION_SHAPE_PATH = "/CollisionShape2D";
	private const int PROJECTILE_SPEED = 300;
	private const float LIFE_TIME = 3f;

	private int _damage;
	private CollisionShape2D _collisionShape;

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>(GetPath() + COLLISION_SHAPE_PATH);
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	public void Fire(int damage)
	{
		_damage = damage;
		ApplyImpulse(new Vector2(), new Vector2(PROJECTILE_SPEED, 0).Rotated(Rotation));
		SelfDestruct();
	}

	public void DisableCollision()
	{
		_collisionShape.SetDeferred("disabled", true);
	}

	public void OnBodyEntered(Node body)
	{
		DisableCollision();
		if (body is IEnemy)
		{
			IEnemy enemy = (IEnemy)body;
			enemy.OnHit(_damage);
		}
		else
		{
			GD.Print("Hit something else");
		}
		Hide();
	}

	public async void SelfDestruct()
	{
		await ToSignal(GetTree().CreateTimer(LIFE_TIME), "timeout");
		QueueFree();
	}
}
