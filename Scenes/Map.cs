using Godot;
using System;
using System.Collections.Generic;

public class Map : Node2D
{
	
	private const string TileMapPath = "/TileMap";
	private const string SpikeTrapPath = "res://Traps/SpikeTrap.tscn";
	private const int TileSize = 32;
	private const int SpikeTrapID = 15;

	private Vector2 tileMargin = new Vector2(TileSize/2, TileSize/2);
	private TileMap tileMap;
	private PackedScene spikeTrapInstance;

	public override void _Ready()
	{
		tileMap = GetNode<TileMap>(GetPath() + TileMapPath);
		spikeTrapInstance = ResourceLoader.Load<PackedScene>(SpikeTrapPath);
		SetTraps(GetTrapLocations());
	}

	private List<Vector2> GetTrapLocations()
	{
		var trapLocations = new List<Vector2>();
		var tilesArray = tileMap.GetUsedCellsById(SpikeTrapID);
		foreach (object arrayObject in tilesArray)
		{
			trapLocations.Add(tileMap.MapToWorld((Vector2)arrayObject));
		}
		return trapLocations;
	}

	private void SetTraps(List<Vector2> trapLocations)
	{
		if (trapLocations.Count == 0)
			return;
		foreach (Vector2 location in trapLocations)
		{
			var trap = (Node2D)spikeTrapInstance.Instance();
			trap.GlobalPosition = location + tileMargin; // 
			AddChild(trap);
		}
	}
}
