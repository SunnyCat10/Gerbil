using Godot;
using System;
using Gerbil;

public class EnemyProjectile : RigidBody2D, IProjectile
{
	private const string COLLISION_SHAPE_PATH = "/CollisionShape2D";
	private const string AnimatedSpritePath = "/AnimatedSprite";
	private const string FlashAnimation = "Flash";
	private const string FlyingAnimation = "Flying";
	private const string OnHitAnimation = "OnHit";
	private const int PROJECTILE_SPEED = 300;
	private const float LIFE_TIME = 3f;

	private int _damage;
	private CollisionShape2D _collisionShape;
	private AnimatedSprite animatedSprite;

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>(GetPath() + COLLISION_SHAPE_PATH);
		animatedSprite = GetNode<AnimatedSprite>(GetPath() + AnimatedSpritePath);
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	public void DisableCollision()
	{
		_collisionShape.SetDeferred("disabled", true);
	}

	public async void Fire(int damage)
	{
		_damage = damage;
		ApplyImpulse(new Vector2(), new Vector2(PROJECTILE_SPEED, 0).Rotated(Rotation));
		animatedSprite.Play(FlashAnimation);
		await ToSignal(animatedSprite, "animation_finished");
		animatedSprite.Play(FlyingAnimation);
		SelfDestruct();
	}

	public void OnBodyEntered(Node body)
	{
		DisableCollision();
		if (body is Player)
		{
			Player player = (Player)body;
			player.Damage(_damage);
			OnHit();
		}
		else if (body is TileMap)
		{
			OnHit();
		}
		else
			_collisionShape.SetDeferred("disabled", false);
	}

	public async void SelfDestruct()
	{
		await ToSignal(GetTree().CreateTimer(LIFE_TIME), "timeout");
		QueueFree();
	}

	private async void OnHit()
	{
		animatedSprite.Play(OnHitAnimation);
		await ToSignal(animatedSprite, "animation_finished");
		Hide();
	}
}
