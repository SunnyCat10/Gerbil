using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using Gerbil.Weapons;

namespace Gerbil
{
	public class WeaponManager : Node2D
	{
		public Position2D WeaponRotationAxis { get; private set; }
		public Position2D WeaponDisplayPoint { get; private set; }
		public MeleeCollisionDetector MeleeCollisionDetector { get; private set; }

		private const string WeaponInventoryUIPath = "/root/Map/CanvasLayer/WeaponInventoryUI";
		private const int WeaponInventorySize = 3;
		private const float MeleeDisplayPointOffset = 45f; 

		private int currentSelection = 0;
		private WeaponInventoryUI weaponInventoryUI;
		private Weapon[] weaponInventory;
		private bool[] isWeaponCooldownComplete;

		/// <summary>
		/// Godot engine requires parameterless constructor for every class that inherits Godot classes.
		/// </summary>
		public WeaponManager() { }

		public WeaponManager(Position2D weaponRotationAxis, Position2D weaponDisplayPoint, MeleeCollisionDetector meleeCollisionDetector)
		{
			weaponInventory = new Weapon[WeaponInventorySize];
			isWeaponCooldownComplete = new bool[WeaponInventorySize];
			WeaponRotationAxis = weaponRotationAxis;
			WeaponDisplayPoint = weaponDisplayPoint;
			MeleeCollisionDetector = meleeCollisionDetector;
		}

		public override void _Ready()
		{
			weaponInventoryUI = (WeaponInventoryUI)GetNode<Control>(WeaponInventoryUIPath);
		}

		public override void _Process(float delta)
		{
			if (GetCurrentWeapon() != null)
			{
				RotateWeapon();
			}
		}

		private void RotateWeapon()
		{
			if (GetCurrentWeapon() is IMelee)
			{
				if (isWeaponCooldownComplete[currentSelection])
				{
					//GetCurrentWeapon().WeaponNode.Scale = new Vector2(-1, 1);
					float angleToMouse = GetAngleTo(GetGlobalMousePosition());
					WeaponRotationAxis.Rotation = angleToMouse; // - 2.356f;	
				}
			}
			else
			{
				float angleToMouse = GetAngleTo(GetGlobalMousePosition());
				if (angleToMouse < 1.57 && angleToMouse > -1.57)
					GetCurrentWeapon().RootNode.Scale = new Vector2(1, 1);
				else
					GetCurrentWeapon().RootNode.Scale = new Vector2(1, -1);
				WeaponRotationAxis.Rotation = angleToMouse;
			}
		}

		/// <summary>
		/// Rotates the weapon display point by 45 degrees when the player is holding a melee weapon.
		/// </summary>
		private void RotateWeaponDisplayPoint()
		{
			if (GetCurrentWeapon() is IMelee)
				WeaponDisplayPoint.Rotation = MeleeDisplayPointOffset * Mathf.Pi / 180f; // = 0.78539f; 
			else
				WeaponDisplayPoint.Rotation = 0f;
		}

		/// <summary>
		/// Updates Melee collision parameters with the current held weapon.
		/// </summary>
		private void UpdateMeleeCollision()
		{
			MeleeWeapon meleeWeapon = (MeleeWeapon)GetCurrentWeapon();
			MeleeCollisionDetector.CircleRadius = meleeWeapon.AttackCircleRadius;
			MeleeCollisionDetector.CollisionArc = meleeWeapon.CollisionArc;
		}

		/// <summary>
		/// Adds a new weapon to the weapon manager.
		/// </summary>
		/// <param name="weapon">New weapon to add.</param>
		public void AddWeapon(Weapon weapon)
		{
			GD.Print("Added Weapon");
			for (int i = 0; i < weaponInventory.Length; i++)
			{
				if (weaponInventory[i] == null)
				{
					if (weaponInventory[currentSelection] != null) // Checks if the player is holding a weapon already.
						weapon.Visible = false;
					weaponInventory[i] = weapon;
					weaponInventoryUI.AddWeapon(weapon, i);
					isWeaponCooldownComplete[i] = true;
					RotateWeaponDisplayPoint();
					if (GetCurrentWeapon() is MeleeWeapon)
						UpdateMeleeCollision();
					return;
				}
			}
			GD.Print("Weapon inventory full -> Implement switching");
			//TODO: Implement weapon switching
		}

		//TODO: Implement weapon removing...
		public void RemoveWeapon(int index)
		{
			if (index >= weaponInventory.Length || index < 0)
				GD.Print("[ERROR] Failed to remove a weapon in non existent index at weaponInventory array.");
			else
				weaponInventory[index] = null;
		}

		/// <summary>
		/// Gets the current weapon.
		/// </summary>
		/// <returns>Current weapon.</returns>
		public Weapon GetCurrentWeapon()
		{
			return weaponInventory[currentSelection];
		}

		public bool ScrollRight()
		{
			weaponInventoryUI.ScrollRight();
			if (weaponInventory[currentSelection] != null)
				weaponInventory[currentSelection].RootNode.Visible = false;
			if (currentSelection + 1 == weaponInventory.Length)
			{
				currentSelection = 0;
			}
			else
			{
				++currentSelection;
			}
			if (weaponInventory[currentSelection] != null)
				weaponInventory[currentSelection].RootNode.Visible = true;
			RotateWeaponDisplayPoint();
			if (GetCurrentWeapon() is MeleeWeapon)
				UpdateMeleeCollision();
			return (weaponInventory[currentSelection] != null);
		}

		public bool ScrollLeft()
		{
			weaponInventoryUI.ScrollLeft();
			if (weaponInventory[currentSelection] != null)
				weaponInventory[currentSelection].RootNode.Visible = false;
			if (currentSelection - 1 == -1)
			{
				currentSelection = weaponInventory.Length - 1;
			}
			else
			{
				--currentSelection;     
			}
			if (weaponInventory[currentSelection] != null)
				weaponInventory[currentSelection].RootNode.Visible = true;
			RotateWeaponDisplayPoint();
			if (GetCurrentWeapon() is MeleeWeapon)
				UpdateMeleeCollision();
			return (weaponInventory[currentSelection] != null);
		}
	 
		public async void Attack()
		{
			int selectedWeapon = currentSelection;
			if (isWeaponCooldownComplete[selectedWeapon])
			{
				Weapon weapon = GetCurrentWeapon();
				if (weapon is IMelee)
				{
					isWeaponCooldownComplete[selectedWeapon] = false;
					IMelee meleeWeapon = (IMelee)weapon;
					await meleeWeapon.Attack();
					await ToSignal(GetTree().CreateTimer(weapon.RateOfFire), "timeout");
					isWeaponCooldownComplete[selectedWeapon] = true;
				}
				else //Weapon is ranged:
				{
					isWeaponCooldownComplete[selectedWeapon] = false;
					RigidBody2D projectile = (RigidBody2D)weapon.ProjectileInstance.Instance();
					projectile.GlobalPosition = weapon.ProjectileSpawnPoint.GlobalPosition;
					projectile.Rotation = WeaponRotationAxis.Rotation; 
					GetParent().GetParent().AddChild(projectile);
					IProjectile projectileInterface = (IProjectile)projectile;
					projectileInterface.Fire(weapon.Damage);
					await ToSignal(GetTree().CreateTimer(weapon.RateOfFire), "timeout");
					isWeaponCooldownComplete[selectedWeapon] = true;
				}
			}
		}
	}
}
