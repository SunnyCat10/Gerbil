using Godot;
using System;
using Gerbil;

public class Bob : KinematicBody2D, IEnemy
{
	private const string PROJECTILE_PATH = "res://Projectiles/EnemyProjectile.tscn";
	private const string COLLLISION_PATH = "/CollisionShape2D";
	private const string PLAYER_PATH = "/root/Map/Player";
	private const string TIMER_PATH = "/Timer";
	private const float DETECTION_RANGE = 200f;
	private const float MOVEMENT_SPEED = 1f;
	private const int PROJECTILE_DAMAGE = 1;

	private bool _canFire = true;
	private int _maxHealth = 300;
	private int _currentHealth;

	private CollisionShape2D _collisionShape;
	private Player _player;
	private Timer _stateTimer;
	private RandomNumberGenerator _rng;

	private Vector2 _movingDirection = Vector2.Zero;
	private PackedScene _projectile;

	private enum State
	{
		Hold,
		PickDirection,
		Move,
	}

	private State _currentState;
	
	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>(GetPath() + COLLLISION_PATH);
		_player = (Player)GetNode<Node2D>(PLAYER_PATH);
		_stateTimer = GetNode<Timer>(GetPath() + TIMER_PATH);
		_currentHealth = _maxHealth;
		_stateTimer.Connect("timeout", this, nameof(OnStateMachineTick));
		_rng = _player.Rng;
		_projectile = ResourceLoader.Load<PackedScene>(PROJECTILE_PATH);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_currentState == State.Move)
			MoveAndCollide(_movingDirection * MOVEMENT_SPEED);
	}

	private void OnStateMachineTick()
	{
		//GD.Print(GlobalPosition.DistanceTo(_player.GlobalPosition));
		if (GlobalPosition.DistanceTo(_player.GlobalPosition) <= DETECTION_RANGE)
		{
			Fire( (_player.GlobalPosition - GlobalPosition).Normalized() );
		}
		else
			StateMachineTick();
	}

	private void StateMachineTick()
	{
		int state = _rng.RandiRange(0, 2);
		switch ((State)state)
		{
			case State.Hold:
				_currentState = State.Hold;
				break;
			case State.PickDirection:
				PickDirection();
				_currentState = State.PickDirection;
				break;
			case State.Move:
				_currentState = State.Move;
				break;
		}
	}
	private void PickDirection()
	{
		int x = _rng.RandiRange(-1, 1);
		int y = _rng.RandiRange(-1, 1);
		_movingDirection = new Vector2(x,y).Normalized();
	}

	private void Fire(Vector2 fireDirection)
	{
		RigidBody2D projectile = (RigidBody2D)_projectile.Instance();
		projectile.GlobalPosition = GlobalPosition;
		projectile.Rotation = fireDirection.Angle();
		GetParent().AddChild(projectile);
		IProjectile projectileInterface = (IProjectile)projectile;
		projectileInterface.Fire(PROJECTILE_DAMAGE);
		//if (_canFire)
		//{
		//	//_canFire = false;
		//	RigidBody2D projectile = (RigidBody2D)_projectile.Instance();
		//	projectile.GlobalPosition = GlobalPosition;
		//	projectile.Rotation = fireDirection.Angle();
		//	GetParent().AddChild(projectile);
		//	IProjectile projectileInterface = (IProjectile)projectile;
		//	projectileInterface.Fire(PROJECTILE_DAMAGE);
		//	//await ToSignal(GetTree().CreateTimer(_currentWeapon.RateOfFire), "timeout");
		//	//_canFire = true;
		//}
	}

	public void OnHit(int damage)
	{
		_currentHealth -= damage;
		if (_currentHealth <= 0)
			OnDeath();
	}

	private void OnDeath()
	{
		_collisionShape.SetDeferred("disabled", true);
		//Animation
		QueueFree();
	}
}
