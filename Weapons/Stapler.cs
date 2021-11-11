using Godot;
using System;
using Gerbil;

public class Stapler : Weapon, IWeapon
{
	private const string WeaponCollisionPath = "/CollisionShape2D";
	private const string WeaponTexturePath = "/Stapler";
	private const string ProjectileSpawnPointPath = "/ProjectileSpawnPoint";
	private const string ProjectileScenePath = "res://Projectiles/Staple.tscn";
	private const float WeaponRateOfFire = 0.5f;
	private const int WeaponDamage = 20;
	private const int WeaponContactCost = 0;

	//private CollisionShape2D _collisionShape;
	//private Weapon weaponStats;

	public override void _Ready()
	{
		//_collisionShape = GetNode<CollisionShape2D>(GetPath() + COLLISION_PATH);
		RateOfFire = WeaponRateOfFire;
		Damage = WeaponDamage;
		ContactCost = WeaponContactCost;
		RootNode = this;
		CollisionBox = GetNode<Node2D>(GetPath() + WeaponCollisionPath);
		Icon = GetNode<Sprite>(GetPath() + WeaponTexturePath).Texture;
		ProjectileSpawnPoint = GetNode<Position2D>(GetPath() + ProjectileSpawnPointPath);
		ProjectileInstance = ResourceLoader.Load<PackedScene>(ProjectileScenePath);
		//weaponStats = new Weapon(
		//	FIRE_RATE,
		//	DAMAGE,
		//	CONTACT_COST,
		//	GetNode<Node2D>(GetPath()),
		//	GetNode<Sprite>(GetPath() + WEAPON_SPRITE_PATH).Texture,
		//	GetNode<Position2D>(GetPath() + PROJECTILE_SPAWN_POINT_PATH),
		//	PROJECTILE_SCENE_PATH);
	}

	public new Weapon OnPickUp(Node2D picker)
	{
		return base.OnPickUp(picker);
		//_collisionShape.SetDeferred("disabled", true);
		//GlobalPosition = new Vector2();
		//GetParent().RemoveChild(this);
		//picker.AddChild(this);
		//return weaponStats;
	}
}
