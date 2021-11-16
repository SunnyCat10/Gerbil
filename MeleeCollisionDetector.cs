using Godot;
using System;
using Gerbil;
using System.Collections.Generic;

public class MeleeCollisionDetector : Area2D
{
	[Signal]
	public delegate void OnCollisionHit(Node2D body);

	public float CollisionArc 
	{
		set => cosineArcDegree = Mathf.Cos(value / 2); 
	}

	public float CircleRadius 
	{
		get => circleShape.Radius - displayPointOffset;
		set => circleShape.Radius = displayPointOffset + value;
	}

	private const string WeaponDisplayPointPath = "/TurnAxis/ProjectileSpawnPoint";
	private const string CircleShapePath = "/CircleShape2D";

	private float displayPointOffset;
	private float cosineArcDegree;
	private CircleShape2D circleShape;
	private List<Node2D> bodiesInsideCircle;


	//TODO for testing only
	int TEST = 0;

	public override void _Ready()
	{
		bodiesInsideCircle = new List<Node2D>();
		displayPointOffset = GetNode<Position2D>(GetParent().GetPath() + WeaponDisplayPointPath).Position.x;
		circleShape = (CircleShape2D)GetNode<CollisionShape2D>(GetPath() + CircleShapePath).Shape;
		Connect("body_entered", this, nameof(OnAreaEnter));
		Connect("body_exited", this, nameof(OnAreaExit));
	}
	public void ActivateCollision(bool isChecking)
	{
		circleShape.SetDeferred("disabled", isChecking);
		if (!isChecking) // Clears the list of bodies inside the collision shape if it will be disabled.
			bodiesInsideCircle = new List<Node2D>();
	}

	public void DetectCollision(Vector2 weaponAttackCenter)
	{
		foreach (Node2D body in bodiesInsideCircle)
		{
			Vector2 collidingBodyDirection = (body.GlobalPosition - GlobalPosition).Normalized(); 
			//GD.Print("COIN " + collidingBodyDirection.ToString());
			if (weaponAttackCenter.Dot(collidingBodyDirection) > cosineArcDegree)
			{
				GD.Print(TEST);
				TEST++;
			}
		}
	}

	private void OnAreaEnter(Node body)
	{
		if (body is IEnemy || body is Quicoin) //Quicoin for testing only.
		{
			GD.Print("ENTERED");
			bodiesInsideCircle.Add((Node2D)body);
		}
	}

	private void OnAreaExit(Node body)
	{
		GD.Print("EXIT");
		int bodyIndex = -1;
		for (int i = 0; i < bodiesInsideCircle.Count; i++)
		{
			if (bodiesInsideCircle[i].Name.Equals(body.Name))
				bodyIndex = i;
		}
		if (bodyIndex != -1)
			bodiesInsideCircle.RemoveAt(bodyIndex);
	}

}
