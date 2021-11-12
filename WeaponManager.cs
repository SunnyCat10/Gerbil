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

		private const string WEAPON_INVENTORY_UI_PATH = "/root/Map/CanvasLayer/WeaponInventoryUI";
		private const int WEAPON_INVENTROY_SIZE = 3;
		private int currentSelection = 0;

		private WeaponInventoryUI _weaponInventoryUI;
		private Weapon[] _weaponInventory;
		private bool[] isWeaponCooldownComplete;

		//V2
		private bool isInMeleeAttack = false;

		/// <summary>
		/// Godot engine requires parameterless constructor for every class that inherits Godot classes.
		/// </summary>
		public WeaponManager() { }

		public WeaponManager(Position2D weaponRotationAxis, Position2D weaponDisplayPoint)
		{
			_weaponInventory = new Weapon[WEAPON_INVENTROY_SIZE];
			isWeaponCooldownComplete = new bool[WEAPON_INVENTROY_SIZE];
			WeaponRotationAxis = weaponRotationAxis;
			WeaponDisplayPoint = weaponDisplayPoint; 
		}

		public override void _Ready()
		{
			_weaponInventoryUI = (WeaponInventoryUI)GetNode<Control>(WEAPON_INVENTORY_UI_PATH);
		}

		public override void _Process(float delta)
		{
			if (GetCurrentWeapon() != null)
			{
				if (isInMeleeAttack)
				{
					// Can stun player if needed over here
				}
				else
				{
					RotateWeapon();
				}
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

		private void RotateWeaponDisplayPoint()
		{
			if (GetCurrentWeapon() is IMelee)
				WeaponDisplayPoint.Rotation = 0.78539f;
			else
				WeaponDisplayPoint.Rotation = 0f;
		}

		public void AddWeapon(Weapon weapon)
		{
			GD.Print("Added Weapon");
			for (int i = 0; i < _weaponInventory.Length; i++)
			{
				if (_weaponInventory[i] == null)
				{
					_weaponInventory[i] = weapon;
					_weaponInventoryUI.AddWeapon(weapon, i);
					isWeaponCooldownComplete[i] = true;
					RotateWeaponDisplayPoint();
					return;
				}
			}
			GD.Print("Weapon inventory full -> Implement switching");
			//TODO: Implement weapon switching
		}

		public void RemoveWeapon(int index)
		{
			if (index >= _weaponInventory.Length || index < 0)
				GD.Print("[ERROR] Failed to remove a weapon in non existent index at weaponInventory array.");
			else
				_weaponInventory[index] = null;
		}

		public Weapon GetCurrentWeapon()
		{
			return _weaponInventory[currentSelection];
		}

		public bool ScrollRight()
		{
			_weaponInventoryUI.ScrollRight();
			if (_weaponInventory[currentSelection] != null)
				_weaponInventory[currentSelection].RootNode.Visible = false;
			if (currentSelection + 1 == _weaponInventory.Length)
			{
				currentSelection = 0;
			}
			else
			{
				++currentSelection;
			}
			if (_weaponInventory[currentSelection] != null)
				_weaponInventory[currentSelection].RootNode.Visible = true;
			RotateWeaponDisplayPoint();
			return (_weaponInventory[currentSelection] != null);
		}

		public bool ScrollLeft()
		{
			_weaponInventoryUI.ScrollLeft();
			if (_weaponInventory[currentSelection] != null)
				_weaponInventory[currentSelection].RootNode.Visible = false;
			if (currentSelection - 1 == -1)
			{
				currentSelection = _weaponInventory.Length - 1;
			}
			else
			{
				--currentSelection;     
			}
			if (_weaponInventory[currentSelection] != null)
				_weaponInventory[currentSelection].RootNode.Visible = true;
			RotateWeaponDisplayPoint();
			return (_weaponInventory[currentSelection] != null);
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
