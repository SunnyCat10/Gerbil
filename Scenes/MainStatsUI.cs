using Godot;
using System;

public class MainStatsUI : Control
{
	private const string HEALTH_CONTAINER_PATH = "/MarginContainer/VBoxContainer/HealthContainer";
	private const string CONTACT_CONTAINER_PATH = "/MarginContainer/VBoxContainer/ContactContainer";
	private const string HEALTH_TEXTURE_PATH = "res://Icons/HeartIcon.png"; 
	private const string CONTACT_TEXTURE_PATH = "res://Icons/ContactIcon.png";
	private const string EMPTY_HEALTH_TEXTURE_PATH = "res://Icons/EmptyHeartIcon.png";
	private const string EMPTY_CONTACCT_TEXTURE_PATH = "res://Icons/EmptyContactIcon.png";

	private const int MAX_HEALTH_ICONS = 9;
	private const int MAX_CONTACT_ICONS = 9;

	private int _maxHealth = 0;
	private int _maxContact = 0;
	private int _health = 0;
	private int _contact = 0;

	private HBoxContainer _healthContainer;
	private HBoxContainer _contactContainer;
	
	private Texture _healthTexture;
	private Texture _contactTexture;
	private Texture _emptyHealthTexture;
	private Texture _emptyContactTexture;

	public override void _Ready()
	{
		_healthContainer = GetNode<HBoxContainer>(GetPath() + HEALTH_CONTAINER_PATH);
		_contactContainer = GetNode<HBoxContainer>(GetPath() + CONTACT_CONTAINER_PATH);
		_healthTexture = ResourceLoader.Load<Texture>(HEALTH_TEXTURE_PATH);
		_contactTexture = ResourceLoader.Load<Texture>(CONTACT_TEXTURE_PATH);
		_emptyHealthTexture = ResourceLoader.Load<Texture>(EMPTY_HEALTH_TEXTURE_PATH);
		_emptyContactTexture = ResourceLoader.Load<Texture>(EMPTY_CONTACCT_TEXTURE_PATH);
	}

	private TextureRect CreateIcon(Texture iconTexture)
	{
		TextureRect icon = new TextureRect();
		icon.Texture = iconTexture;
		return icon;
	}

	private void AddHealthContainer()
	{
		if (_maxHealth + 1 > MAX_HEALTH_ICONS)
			GD.Print("ERROR: You can`t add more health icons than can be displayed on the UI");
		else
		{
			TextureRect icon = CreateIcon(_healthTexture);
			icon.Name = _maxHealth.ToString();
			_healthContainer.AddChild(icon);
			++_maxHealth;
		}
	}

	private void AddContactContainer()
	{
		if (_maxContact + 1 > MAX_CONTACT_ICONS)
			GD.Print("ERROR: You can`t add more contact icons than can be displayed on the UI");
		else
		{
			TextureRect icon = CreateIcon(_contactTexture);
			icon.Name = _maxContact.ToString();
			_contactContainer.AddChild(icon);
			++_maxContact;
		}
	}

	/// <summary>
	/// Updates a specfic icon inside an icon container (for example specific heart in the health icon container).
	/// </summary>
	/// <param name="container">Icon container</param>
	/// <param name="iconIndex">Index of the icon inside the container</param>
	/// <param name="updatedTexture">Texture to update to</param>
	private void UpdateContainerIcon(HBoxContainer container, int iconIndex, Texture updatedTexture)
	{
		TextureRect icon = (TextureRect)container.GetChild(iconIndex);
		icon.Texture = updatedTexture;
	}

	public void SetupStartingStats(int health, int contact)
	{
		for (int i = 0; i < health; i++)
		{
			AddHealthContainer();
			++_health;
		}
		for (int i = 0; i < contact; i++)
		{
			AddContactContainer();
			++_contact;
		}
	}

	public void RemoveHealth(int amount)
	{
		int containerIndex = _health - 1;
		for (int i = 0; i < amount;  i++)
		{
			UpdateContainerIcon(_healthContainer, containerIndex, _emptyHealthTexture);
			--containerIndex;
			--_health;
		}
	}

	public void AddHealth(int amount)
	{
		int containerIndex = _health - 1;
		for (int i = 0; i < amount; i++)
		{
			UpdateContainerIcon(_healthContainer, containerIndex, _healthTexture);
			++containerIndex;
			++_health;
		}
	}

	public void RemoveContact(int amount)
	{
		int containerIndex = _contact - 1;
		for (int i = 0; i < amount; i++)
		{
			UpdateContainerIcon(_contactContainer, containerIndex, _emptyContactTexture);
			--containerIndex;
			--_contact;
		}
	}

	public void AddContact(int amount)
	{
		int containerIndex = _contact - 1;
		for (int i = 0; i < amount; i++)
		{
			UpdateContainerIcon(_contactContainer, containerIndex, _contactTexture);
			++containerIndex;
			++_contact;
		}
	}
}
