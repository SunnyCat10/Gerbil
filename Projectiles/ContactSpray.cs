using Godot;
using System;
using Gerbil;

public class ContactSpray : RigidBody2D, IProjectile
{
	private int _projectileSpeed = 500;
	private float _lifeTime = 3f;
	private int _damage;
	private CollisionShape2D _collisionShape;
	public override void _Ready()
	{
		Player player = (Player)GetNode<Node>("/root/Map/Player");
		_damage = player.GetCurrentWeapon().Damage;
		_collisionShape = GetNode<CollisionShape2D>(GetPath() + "/CollisionShape2D");
		Connect("body_entered", this, nameof(OnBodyEntered));
		ApplyImpulse(new Vector2(), new Vector2(_projectileSpeed, 0).Rotated(Rotation));
		SelfDestruct();
	}

	public void Fire(int damage)
	{
		throw new NotImplementedException();
	}

	public async void SelfDestruct()
	{
		await ToSignal(GetTree().CreateTimer(_lifeTime), "timeout");
		QueueFree();
	}

	public void DisableCollision()
	{
		_collisionShape.SetDeferred("disabled", true);
	}

	public void OnBodyEntered(Node body)
	{
		DisableCollision();
		if (body.IsInGroup("Enemies"))
		{
			IEnemy enemy = (IEnemy)body;
			enemy.OnHit(_damage);
		}
		Hide();
	} 
}
