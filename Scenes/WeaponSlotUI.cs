using Godot;
using System;

public class WeaponSlotUI : TextureRect
{
	private const string WEAPON_TEXTURE_RECT_PATH = "/MarginContainer/WeaponTexture";
	private const string EMPTY_SLOT_ICON_PATH = "res://Icons/WeaponInventoryBackground.png";
	private const string SELECTED_SLOT_ICON_PATH = "res://Icons/WeaponInventoryBackgroundSelected.png";

	private Texture _emptySlotTexture;
	private Texture _selectedSlotTexture;
	private TextureRect _weaponTexture;

	public override void _Ready()
	{
		_weaponTexture = GetNode<TextureRect>(GetPath() + WEAPON_TEXTURE_RECT_PATH);
		_emptySlotTexture = ResourceLoader.Load<Texture>(EMPTY_SLOT_ICON_PATH);
		_selectedSlotTexture = ResourceLoader.Load<Texture>(SELECTED_SLOT_ICON_PATH);
	}

	public void Select(bool select)
	{
		if (select)
			Texture = _selectedSlotTexture;
		else
			Texture = _emptySlotTexture;
	}

	public void AddWeapon(Texture texture)
	{
		_weaponTexture.Texture = texture;
	}
}
