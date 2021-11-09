using Godot;
using System;
using Gerbil;
using Gerbil.Weapons;

public class Player : KinematicBody2D
{
	private const float BASE_SPEED = 2.5f;

	[Export]
	private float MAX_SPEED = 2.9f;
	[Export]
	private int ACCELERATION = 30;
	private Vector2 _motion = Vector2.Zero;

	private bool _alive = true;
	private int _maxHealth;
	private int _health;
	private int _maxContact;
	private int _contact;
	private int _speed;
	private int _luck;
	private float _magicModifier;
	private float _rangedModifier;
	private float _meeleModifier;


	private MainStatsUI _mainStatsUI;
	private Position2D _turnAxis;
	private Position2D _projectileSpawnPoint;

	public WeaponManager WeaponManager { get; private set; }
	private Weapon _currentWeapon;
	private bool _canFire = true;

	private Node2D _weaponRootNode;

	//TODO: TEST
	public static bool Stun { get; set; } = false;

	private bool _canGetCollisionDamage = true;

	public RandomNumberGenerator Rng { get; private set; }

	public override async void _Ready()
	{
		//TODO: TEMP REMOVE LATER1!!!!!!!1
		//OS.WindowFullscreen = !OS.WindowFullscreen;

		_mainStatsUI = GetNode<MainStatsUI>("/root/Map/CanvasLayer/MainStatsUI");
		await ToSignal(_mainStatsUI, "ready");
		LoadCharacter(3,9);
		_mainStatsUI.SetupStartingStats(_maxHealth, _maxContact);

		_turnAxis = GetNode<Position2D>("/root/Map/Player/TurnAxis");
		_projectileSpawnPoint = (Position2D)_turnAxis.GetChild(0);

		WeaponManager = new WeaponManager(_turnAxis ,_projectileSpawnPoint);
		AddChild(WeaponManager);

		Rng = new RandomNumberGenerator();
		Rng.Seed = (ulong)GD.Hash("Gishpull" + GetInstanceId().ToString());
	}

	public override void _Process(float delta)
	{
		if (_alive)
		{
			HandleScrollingMovement();
			HandleShootingInput(delta);
			//if (_weaponManager.GetCurrentWeapon() != null)
			//{
			//	if (!Stun)
			//		RotateWeapon();
			//}
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_alive)
		{
			KinematicCollision2D collision = HandleMovementInput(delta);
			if (collision != null)
			{
				if (collision.Collider is IWeapon)
				{
					IWeapon weapon = (IWeapon)collision.Collider;
					Weapon pickedWeapon = weapon.OnPickUp(_projectileSpawnPoint);
					WeaponManager.AddWeapon(pickedWeapon);
				}
				else if (collision.Collider is TileMap)
					return;
				else
					OnEnemyCollision();
			}
		}		
	}

	private KinematicCollision2D HandleMovementInput(float delta)
	{
		Vector2 direction = GetDirectionInput();
		if (direction == Vector2.Zero)
			ApplyFriction(ACCELERATION * delta);
		else
			ApplyForce(direction * ACCELERATION * delta);
		return MoveAndCollide(_motion);
	}

	private Vector2 GetDirectionInput()
	{
		Vector2 direction = Vector2.Zero;
		if (Input.IsActionPressed("ui_up"))
			direction.y = -1;
		if (Input.IsActionPressed("ui_right"))
			direction.x = 1;
		if (Input.IsActionPressed("ui_down"))
			direction.y = 1;
		if (Input.IsActionPressed("ui_left"))
			direction.x = -1;
		return direction.Normalized();
	}

	private void ApplyFriction(float amount)
	{
		if (_motion.Length() > amount)
			_motion -= _motion.Normalized() * amount;
		else
			_motion = Vector2.Zero;
	}

	private void ApplyForce(Vector2 acceleration)
	{
		_motion += acceleration;
		_motion = _motion.Clamped(MAX_SPEED);
	}

	private void HandleScrollingMovement()
	{
		if (Input.IsActionJustReleased("scroll_up"))
		{
			if (WeaponManager.GetCurrentWeapon() != null)
				WeaponManager.GetCurrentWeapon().WeaponNode.Visible = false;
			if (WeaponManager.ScrollRight())
				WeaponManager.GetCurrentWeapon().WeaponNode.Visible = true;
		}
		if (Input.IsActionJustReleased("scroll_down"))
		{
			if (WeaponManager.GetCurrentWeapon() != null)
				WeaponManager.GetCurrentWeapon().WeaponNode.Visible = false;
			if (WeaponManager.ScrollLeft())
				WeaponManager.GetCurrentWeapon().WeaponNode.Visible = true;
		}
	}

	private async void HandleShootingInput(float delta)
	{
		if (WeaponManager.GetCurrentWeapon() != null)
		{
			_currentWeapon = WeaponManager.GetCurrentWeapon();
			if (Input.IsActionPressed("Shoot") && _currentWeapon! is IMelee && _canFire)
			{
				_canFire = false;
				WeaponManager.Attack();
				await ToSignal(GetTree().CreateTimer(_currentWeapon.RateOfFire), "timeout");
				_canFire = true;
				return;
			}
			if (Input.IsActionPressed("Shoot") &&
				_canFire && DrainContact(_currentWeapon.ContactCost))
			{
				_canFire = false;

				RigidBody2D projectile = (RigidBody2D)_currentWeapon.ProjectileInstance.Instance();
				projectile.GlobalPosition = _currentWeapon.ProjectileSpawnPoint.GlobalPosition;
				projectile.Rotation = _turnAxis.Rotation;
				GetParent().AddChild(projectile);
				IProjectile projectileInterface = (IProjectile)projectile;
				projectileInterface.Fire(_currentWeapon.Damage);
				await ToSignal(GetTree().CreateTimer(_currentWeapon.RateOfFire), "timeout");
				_canFire = true;
			}
		}
	}
	 
	private async void OnEnemyCollision()
	{
		if (_canGetCollisionDamage)
		{
			_canGetCollisionDamage = false;
			Damage(1);	
			await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
			_canGetCollisionDamage = true;
		}
	}

	//TODO: Make it read from json file.S
	public void LoadCharacter(int health, int contact)
	{
		if (health == 0)
		{
			GD.Print("[ERROR] The character was loaded with 0 health.");
			Death();
			return;
		}
		_maxHealth = health;
		_maxContact = contact;
		_health = _maxHealth;
		_contact = _maxContact;
		_speed = 0;
		_luck = 0;
		_magicModifier = 1;
		_rangedModifier = 1;
		_meeleModifier = 1;
	}

	public void Death()
	{
		_alive = false;
		GD.Print("RIP");
	}

	public void Heal(int healAmount)
	{
		if (_health == _maxHealth)
			return;
		else
		{
			_health += (healAmount + _health) - _maxHealth;
			_mainStatsUI.AddHealth(_health);
		}
	}

	public void Damage(int damageAmount)
	{
		if (_health - damageAmount <= 0)
		{
			_mainStatsUI.RemoveHealth(_health);
			_health = 0;
			Death();
		}
		else
		{
			_health -= damageAmount;
			_mainStatsUI.RemoveHealth(damageAmount);
		}
	}

	public void FillContact(int amount)
	{
		if (_contact == _maxContact)
			return;
		else
		{
			_contact += (amount + _contact) - _maxContact;
			_mainStatsUI.AddContact(_contact);
		}
	}

	public bool DrainContact(int amount)
	{
		if (_contact - amount < 0)
		{
			//TODO: InsufficentContactPopup();
			return false;
		}
		else
		{
			_contact -= amount;
			_mainStatsUI.RemoveContact(amount);
			return true;
		}
	}

	public Weapon GetCurrentWeapon()
	{
		return WeaponManager.GetCurrentWeapon();
	}
}
