using Godot;
using System;
using Gerbil.Weapons;
using Gerbil;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Superbroom : MeleeWeapon, IMelee
{
	private const string WeaponTexturePath = "/AnimatedSprite";
	private const string WeaponCollisionPath = "/CollisionShape2D";
	private const float WeaponRateOfFire = 0.5f;
	private const float WeaponArc = 1.57079f; // 90 Degrees
	private const float WeaponAttackRadius = 30f;
	private const int WeaponDamage = 30;
	private const int WeaponContactCost = 0;
	
	private CollisionShape2D weaponHitboxCollisionShape;
	private WeaponManager playerWeaponManager;

	private Superbroom() { }

	public override void _Ready()
	{
		RateOfFire = WeaponRateOfFire;
		Damage = WeaponDamage;
		ContactCost = WeaponContactCost;
		RootNode = this;
		CollisionBox = GetNode<Node2D>(GetPath() + WeaponCollisionPath);
		WeaponAnimatedSprite = GetNode<AnimatedSprite>(GetPath() + WeaponTexturePath);
		Icon = WeaponAnimatedSprite.Frames.GetFrame("Attack", 0);
		CollisionArc = WeaponArc;
		AttackCircleRadius = WeaponAttackRadius;
	}

	public async Task Attack()
	{
		WeaponAnimatedSprite.Play("Attack");
		playerWeaponManager.MeleeCollisionDetector.DetectCollision(
			(playerWeaponManager.WeaponDisplayPoint.GlobalPosition - playerWeaponManager.WeaponRotationAxis.GlobalPosition).Normalized(),
			Damage);
		await ToSignal(WeaponAnimatedSprite, "animation_finished");
		WeaponAnimatedSprite.Play("Back");
		await ToSignal(WeaponAnimatedSprite, "animation_finished");
	}

	public void ChargedAttack()
	{
		throw new NotImplementedException();
	}

	public new Weapon OnPickUp(Node2D picker)
	{
		if (picker is Player)
		{
			Player player = (Player)picker;
			playerWeaponManager = player.WeaponManager;
		}
		return base.OnPickUp(picker);
	}
}
