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

		private int _currentSelection = 0;

		private WeaponInventoryUI _weaponInventoryUI;
		private Weapon[] _weaponInventory;

		//V2
		private bool isInMeleeAttack = false;
		private float currentRotationInArc = 0;
		private float rotationSpeed = 0.1f;
		private float swingArc = 2.1f;

		//V3 -> When getting new weapon cut swing arc by half
		private float swingArcV2 = 2f;

		public WeaponManager(Position2D weaponRotationAxis, Position2D weaponDisplayPoint)
		{
			_weaponInventory = new Weapon[WEAPON_INVENTROY_SIZE];
			WeaponRotationAxis = weaponRotationAxis;
			WeaponDisplayPoint = weaponDisplayPoint; 
		}

		//public WeaponManager(Weapon startingWeapon)
		//{
		//    _weaponInventory = new Weapon[WEAPON_INVENTROY_SIZE];
		//    _weaponInventory[0] = startingWeapon;
		//}

		public override void _Ready()
		{
			_weaponInventoryUI = (WeaponInventoryUI)GetNode<Control>(WEAPON_INVENTORY_UI_PATH);
		}

		//public void AddWeapon(Weapon weapon, int index)
		//{
		//    if (index >= _weaponInventory.Length || index < 0)
		//        GD.Print("[ERROR] Failed to add a weapon in non existent index at weaponInventory array.");
		//    else
		//        _weaponInventory[index] = weapon;
		//}

		public override void _Process(float delta)
		{
			if (GetCurrentWeapon() != null)
			{
				if (isInMeleeAttack)
				{
					////if (currentRotationInArc >= finalRotationInArc)
					////{
					////    isInMeleeAttack = false;
					////    GD.Print("Gish");
					////}

					//if (currentRotationInArc >= swingArc)
					//{
					//    isInMeleeAttack = false;
					//    GD.Print("Gish");
					//}
					//else
					//{
					//    currentRotationInArc += rotationSpeed;
					//    WeaponRotationAxis.Rotation = WeaponRotationAxis.Rotation + rotationSpeed;

					//    //    float angleToMouse = GetAngleTo(GetGlobalMousePosition());
					//    //    if (angleToMouse < 1.57 && angleToMouse > -1.57)
					//    //        _weaponManager.GetCurrentWeapon().WeaponNode.Scale = new Vector2(1, 1);
					//    //    else
					//    //        _weaponManager.GetCurrentWeapon().WeaponNode.Scale = new Vector2(1, -1);
					//    //    _turnAxis.Rotation = angleToMouse;
					//    //}
					//}
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
				//GetCurrentWeapon().WeaponNode.Scale = new Vector2(-1, 1);
				float angleToMouse = GetAngleTo(GetGlobalMousePosition());
				WeaponRotationAxis.Rotation = angleToMouse; // - 2.356f;
			}
			else
			{
				float angleToMouse = GetAngleTo(GetGlobalMousePosition());
				if (angleToMouse < 1.57 && angleToMouse > -1.57)
					GetCurrentWeapon().WeaponNode.Scale = new Vector2(1, 1);
				else
					GetCurrentWeapon().WeaponNode.Scale = new Vector2(1, -1);
				WeaponRotationAxis.Rotation = angleToMouse;
			}
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
					//_weaponInventory[i].GlobalPosition = WeaponDisplayPoint.GlobalPosition;
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
			return _weaponInventory[_currentSelection];
		}

		public bool ScrollRight()
		{
			_weaponInventoryUI.ScrollRight();
			if (_weaponInventory[_currentSelection] != null)
				_weaponInventory[_currentSelection].WeaponNode.Visible = false;
			if (_currentSelection + 1 == _weaponInventory.Length)
			{
				_currentSelection = 0;
			}
			else
			{
				++_currentSelection;
			}
			if (_weaponInventory[_currentSelection] != null)
				_weaponInventory[_currentSelection].WeaponNode.Visible = true;
			return (_weaponInventory[_currentSelection] != null);
		}

		public bool ScrollLeft()
		{
			_weaponInventoryUI.ScrollLeft();
			if (_weaponInventory[_currentSelection] != null)
				_weaponInventory[_currentSelection].WeaponNode.Visible = false;
			if (_currentSelection - 1 == -1)
			{
				_currentSelection = _weaponInventory.Length - 1;
			}
			else
			{
				--_currentSelection;     
			}
			if (_weaponInventory[_currentSelection] != null)
				_weaponInventory[_currentSelection].WeaponNode.Visible = true;
			return (_weaponInventory[_currentSelection] != null);
		}
	 
		public async void Attack()
		{
			if (GetCurrentWeapon() is IMelee)
			{
				IMelee weapon = (IMelee)GetCurrentWeapon();
				//currentRotationInArc = WeaponRotationAxis.Rotation;
				//finalRotationInArc = currentRotationInArc + swingArc;
				//if (finalRotationInArc > 3.14f)
				//{
				//    finalRotationInArc = 3.14f - finalRotationInArc;
				//}
				//GD.Print(currentRotationInArc);
				//GD.Print(finalRotationInArc);
				//currentRotationInArc = 0;
				//isInMeleeAttack = true;
				//Player.Stun = true;
				//WeaponRotationAxis.Rotation = GetAngleTo(GetGlobalMousePosition()) - swingArcV2/2;
				isInMeleeAttack = true;
				Player.Stun = true;
				//WeaponRotationAxis.Rotation -= 2.356f;
				WeaponRotationAxis.Rotation += 0.78539f;
				await weapon.Attack();
				isInMeleeAttack = false;
				Player.Stun = false;
			}
		}
	}
}
