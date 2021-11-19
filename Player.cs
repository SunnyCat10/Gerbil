using Godot;
using System;
using Gerbil;
using Gerbil.Props;
using Gerbil.Consumables;

public class Player : KinematicBody2D
{
	public WeaponManager WeaponManager { get; private set; }
	public RandomNumberGenerator Rng { get; private set; }

	private const string MainStatsUIPath = "/root/Map/CanvasLayer/MainStatsUI";
	private const string TurnAxisPath = "/root/Map/Player/TurnAxis";
	private const string MeeleCollisionDetectorPath = "/MeleeCollisionDetector";

	[Export]
	private float MAX_SPEED = 2.9f;
	[Export]
	private int ACCELERATION = 30;
	private Vector2 motion = Vector2.Zero;

	private bool alive = true;
	private int maxHealth;
	private int health;
	private int maxContact;
	private int contact;
	private int _speed;
	private int _luck;
	private float _magicModifier;
	private float _rangedModifier;
	private float _meeleModifier;
	private bool canGetCollisionDamage = true;
	private MainStatsUI mainStatsUI;
	private Position2D turnAxis;
	private Position2D projectileSpawnPoint;

	public override async void _Ready()
	{
		mainStatsUI = GetNode<MainStatsUI>(MainStatsUIPath);
		await ToSignal(mainStatsUI, "ready");
		LoadCharacter(3,9);
		mainStatsUI.SetupStartingStats(maxHealth, maxContact);

		turnAxis = GetNode<Position2D>(TurnAxisPath);
		projectileSpawnPoint = (Position2D)turnAxis.GetChild(0);

		WeaponManager = new WeaponManager(turnAxis ,projectileSpawnPoint, (MeleeCollisionDetector)GetNode<Node2D>(GetPath() + MeeleCollisionDetectorPath));
		AddChild(WeaponManager);

		Rng = new RandomNumberGenerator();
		Rng.Seed = (ulong)GD.Hash("Gishpull" + GetInstanceId().ToString());	
	}

	public override void _Process(float delta)
	{
		if (alive)
		{
			HandleScrollingMovement();
			HandleShootingInput(delta);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (alive)
		{
			KinematicCollision2D collision = HandleMovementInput(delta);
			if (collision != null)
			{
				if (collision.Collider is IWeapon)
				{
					IWeapon weapon = (IWeapon)collision.Collider;
					Weapon pickedWeapon = weapon.OnPickUp(this);
					WeaponManager.AddWeapon(pickedWeapon);
				}
				else if (collision.Collider is TileMap || collision.Collider is IBreakable)
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
		return MoveAndCollide(motion);
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
		if (motion.Length() > amount)
			motion -= motion.Normalized() * amount;
		else
			motion = Vector2.Zero;
	}

	private void ApplyForce(Vector2 acceleration)
	{
		motion += acceleration;
		motion = motion.Clamped(MAX_SPEED);
	}

	private void HandleScrollingMovement()
	{
		if (Input.IsActionJustReleased("scroll_down")) 
		{
			if (WeaponManager.GetCurrentWeapon() != null)
				WeaponManager.GetCurrentWeapon().RootNode.Visible = false;
			if (WeaponManager.ScrollRight())
				WeaponManager.GetCurrentWeapon().RootNode.Visible = true;
		}
		if (Input.IsActionJustReleased("scroll_up"))
		{
			if (WeaponManager.GetCurrentWeapon() != null)
				WeaponManager.GetCurrentWeapon().RootNode.Visible = false;
			if (WeaponManager.ScrollLeft())
				WeaponManager.GetCurrentWeapon().RootNode.Visible = true;
		}
	}

	private void HandleShootingInput(float delta)
	{
		if (WeaponManager.GetCurrentWeapon() != null && Input.IsActionPressed("Shoot"))
			WeaponManager.Attack();
	}
	 
	private async void OnEnemyCollision()
	{
		if (canGetCollisionDamage)
		{
			canGetCollisionDamage = false;
			Damage(1);	
			await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
			canGetCollisionDamage = true;
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
		maxHealth = health;
		maxContact = contact;
		this.health = maxHealth;
		this.contact = maxContact;
		_speed = 0;
		_luck = 0;
		_magicModifier = 1;
		_rangedModifier = 1;
		_meeleModifier = 1;
	}

	public void Death()
	{
		alive = false;
		GD.Print("RIP");
	}

	public void Heal(int healAmount)
	{
		if (health == maxHealth)
			return;
		else
		{
			health += (healAmount + health) - maxHealth;
			mainStatsUI.AddHealth(health);
		}
	}

	public void Damage(int damageAmount)
	{
		if (health - damageAmount <= 0)
		{
			mainStatsUI.RemoveHealth(health);
			health = 0;
			Death();
		}
		else
		{
			health -= damageAmount;
			mainStatsUI.RemoveHealth(damageAmount);
		}
	}

	public void FillContact(int amount)
	{
		if (contact == maxContact)
			return;
		else
		{
			contact += (amount + contact) - maxContact;
			mainStatsUI.AddContact(contact);
		}
	}

	public bool DrainContact(int amount)
	{
		if (contact - amount < 0)
		{
			//TODO: InsufficentContactPopup();
			return false;
		}
		else
		{
			contact -= amount;
			mainStatsUI.RemoveContact(amount);
			return true;
		}
	}

	public Weapon GetCurrentWeapon()
	{
		return WeaponManager.GetCurrentWeapon();
	}
}
