using Godot;
using System;
using Gerbil;

public class Stapler : StaticBody2D, IWeapon
{
	private const string COLLISION_PATH = "/CollisionShape2D";
	private const string WEAPON_SPRITE_PATH = "/Stapler";
	private const string PROJECTILE_SPAWN_POINT_PATH = "/ProjectileSpawnPoint";
	private const string PROJECTILE_SCENE_PATH = "res://Projectiles/Staple.tscn";
	private const float FIRE_RATE = 0.5f;
	private const int DAMAGE = 20;
	private const int CONTACT_COST = 0;

	private CollisionShape2D _collisionShape;
	private Weapon weaponStats;

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>(GetPath() + COLLISION_PATH);
		weaponStats = new Weapon(
			FIRE_RATE,
			DAMAGE,
			CONTACT_COST,
			GetNode<Node2D>(GetPath()),
			GetNode<Sprite>(GetPath() + WEAPON_SPRITE_PATH).Texture,
			GetNode<Position2D>(GetPath() + PROJECTILE_SPAWN_POINT_PATH),
			PROJECTILE_SCENE_PATH);
	}

	public Weapon OnPickUp(Node2D picker)
	{
		_collisionShape.SetDeferred("disabled", true);
		GlobalPosition = new Vector2();
		GetParent().RemoveChild(this);	
		picker.AddChild(this);
		return weaponStats; 
	}
}
