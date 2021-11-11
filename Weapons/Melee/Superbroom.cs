using Godot;
using System;
using Gerbil.Weapons;
using Gerbil;
using System.Threading.Tasks;

public class Superbroom : Weapon, IMelee
{
	private const string WeaponTexturePath = "/AnimatedSprite";
	private const string WeaponCollisionPath = "/CollisionShape2D";
	private const float WeaponRateOfFire = 0.5f;
	private const int WeaponDamage = 30;
	private const int WeaponContactCost = 0;

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
	}

	public async Task Attack()
	{
		WeaponAnimatedSprite.Play("Attack");
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
		return base.OnPickUp(picker);
	}
}
