using Godot;
using System;
using Gerbil.Weapons;
using Gerbil;
using System.Threading.Tasks;

public class Superbroom : Weapon, IMelee
{
	//private const string WeaponTexturePath = "/Sprite";
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
		WeaponNode = this;
		//WeaponTexture = (Sprite)GetNode<Node2D>(GetPath() + WeaponTexturePath);
		CollisionBox = GetNode<Node2D>(GetPath() + WeaponCollisionPath);
		WeaponSprite = GetNode<AnimatedSprite>(GetPath() + WeaponTexturePath);
		WeaponTexture = WeaponSprite.Frames.GetFrame("Attack",0);
	}

	public async Task Attack()
	{
		WeaponSprite.Play("Attack");
		await ToSignal(WeaponSprite, "animation_finished");
		WeaponSprite.Play("Back");
		await ToSignal(WeaponSprite, "animation_finished");
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
