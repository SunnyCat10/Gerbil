using Godot;
using System;
using Gerbil.Weapons;
using Gerbil;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Superbroom : Weapon, IMelee
{
	private const string WeaponTexturePath = "/AnimatedSprite";
	private const string WeaponCollisionPath = "/CollisionShape2D";
	private const string WeaponHitboxPath = "/Area2D";
	private const string WeaponHitboxCollisionShapePath = "/Area2D/CollisionShape2D";
	private const float WeaponRateOfFire = 0.5f;
	private const int WeaponDamage = 30;
	private const int WeaponContactCost = 0;
	
	//private Area2D weaponHitbox;
	private CollisionShape2D weaponHitboxCollisionShape;
	//private Position2D playerWeaponRotationAxis;
	private WeaponManager playerWeaponManager;
	private Vector2 weaponAttackDirection;
	private bool isCheckingForWeaponCollision = false;
	//private List<Node2D> bodiesInsideDetectionRange;

	private int TEST = 0;

	private Superbroom() { }

	public override void _Ready()
	{
		//bodiesInsideDetectionRange = new List<Node2D>();
		RateOfFire = WeaponRateOfFire;
		Damage = WeaponDamage;
		ContactCost = WeaponContactCost;
		RootNode = this;
		CollisionBox = GetNode<Node2D>(GetPath() + WeaponCollisionPath);
		WeaponAnimatedSprite = GetNode<AnimatedSprite>(GetPath() + WeaponTexturePath);
		//weaponHitbox = GetNode<Area2D>(GetPath() + WeaponHitboxPath);
		weaponHitboxCollisionShape = GetNode<CollisionShape2D>(GetPath() + WeaponHitboxCollisionShapePath);
		Icon = WeaponAnimatedSprite.Frames.GetFrame("Attack", 0);
		//weaponHitbox.Connect("body_entered", this, nameof(OnAreaEnter));
		//weaponHitbox.Connect("body_exited", this, nameof(OnAreaExit));
	}

	public override void _Process(float delta)
	{
		if (isCheckingForWeaponCollision)
		{
			
		}
	}

	//private void DetectCollisions()
	//{
	//	foreach (Node2D body in bodiesInsideDetectionRange)
	//	{
	//		Vector2 collidingBodyDirection = (body.GlobalPosition - playerWeaponManager.WeaponDisplayPoint.GlobalPosition).Normalized();
	//		GD.Print("COIN " + collidingBodyDirection.ToString());
	//		if (weaponAttackDirection.Dot(collidingBodyDirection) > 0)
	//		{
	//			GD.Print(TEST);
	//			TEST++;
	//		}
	//	}
	//}

	public async Task Attack()
	{
		//weaponAttackDirection = new Vector2(
		//		Mathf.Sin(playerWeaponManager.WeaponRotationAxis.Rotation),
		//		Mathf.Cos(playerWeaponManager.WeaponRotationAxis.Rotation));
		//weaponAttackDirection = (playerWeaponManager.WeaponDisplayPoint.GlobalPosition - playerWeaponManager.WeaponRotationAxis.GlobalPosition).Normalized();
		//GD.Print(weaponAttackDirection.ToString());
		isCheckingForWeaponCollision = true;
		//DetectCollisions();
		WeaponAnimatedSprite.Play("Attack");
		playerWeaponManager.MeleeCollisionDetector.DetectCollision(
			(playerWeaponManager.WeaponDisplayPoint.GlobalPosition - playerWeaponManager.WeaponRotationAxis.GlobalPosition).Normalized());
		await ToSignal(WeaponAnimatedSprite, "animation_finished");
		WeaponAnimatedSprite.Play("Back");
		await ToSignal(WeaponAnimatedSprite, "animation_finished");
		weaponAttackDirection = Vector2.Zero;
		isCheckingForWeaponCollision = false;
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

	//TODO:: Add collision detection on and off method -> so the weapon will search for collisions only when weapon is equiped.
	public void SetWeaponCollisionCheck(bool isCheckingForCollision)
	{
		weaponHitboxCollisionShape.SetDeferred("disabled", isCheckingForCollision);
	}

	//private void OnAreaEnter(Node body)
	//{
	//	if (body is IEnemy || body is Quicoin) //Quicoin for testing only.
	//	{
	//		GD.Print("ENTERED");
	//		bodiesInsideDetectionRange.Add((Node2D)body);
	//	}
	//}

	//private void OnAreaExit(Node body)
	//{
	//	GD.Print("EXIT");
	//	int bodyIndex = -1;
	//	for(int i = 0; i < bodiesInsideDetectionRange.Count; i++)
	//	{
	//		if (bodiesInsideDetectionRange[i].Name.Equals(body.Name))
	//			bodyIndex = i;
	//	}
	//	if (bodyIndex != -1)
	//		bodiesInsideDetectionRange.RemoveAt(bodyIndex);
	//	else
	//		return;
	//}
}
