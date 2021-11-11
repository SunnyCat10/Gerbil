using Godot;
using System;
using Gerbil;

public class WeaponInventoryUI : Control
{
	private const string WEAPON_SLOT_1_PATH = "/MarginContainer/HBoxContainer/WeaponSlotUI";
	private const string WEAPON_SLOT_2_PATH = "/MarginContainer/HBoxContainer/WeaponSlotUI2";
	private const string WEAPON_SLOT_3_PATH = "/MarginContainer/HBoxContainer/WeaponSlotUI3";

	private int _currentIndex = 0;
	private WeaponSlotUI[] _weaponSlots = new WeaponSlotUI[3];
	

	public override void _Ready()
	{
		AddWeaponSlots();
		_weaponSlots[_currentIndex].Select(true);
	}

	private void AddWeaponSlots()
	{
		_weaponSlots[0] = (WeaponSlotUI)GetNode<Control>(GetPath() + WEAPON_SLOT_1_PATH);
		_weaponSlots[1] = (WeaponSlotUI)GetNode<Control>(GetPath() + WEAPON_SLOT_2_PATH);
		_weaponSlots[2] = (WeaponSlotUI)GetNode<Control>(GetPath() + WEAPON_SLOT_3_PATH);
	}

	public void ScrollRight()
	{
		if (_currentIndex + 1 == _weaponSlots.Length)
		{
			_weaponSlots[_currentIndex].Select(false);
			_currentIndex = 0;
			_weaponSlots[_currentIndex].Select(true);
		}
		else
		{
			_weaponSlots[_currentIndex].Select(false);
			++_currentIndex;
			_weaponSlots[_currentIndex].Select(true);
		}
	}

	public void ScrollLeft()
	{
		if (_currentIndex - 1 == -1)
		{
			_weaponSlots[_currentIndex].Select(false);
			_currentIndex = _weaponSlots.Length - 1;
			_weaponSlots[_currentIndex].Select(true);
		}
		else
		{
			_weaponSlots[_currentIndex].Select(false);
			--_currentIndex;
			_weaponSlots[_currentIndex].Select(true);
		}
	}

	public void AddWeapon(Weapon weapon, int index)
	{
		_weaponSlots[index].AddWeapon(weapon.Icon);
	}
}
