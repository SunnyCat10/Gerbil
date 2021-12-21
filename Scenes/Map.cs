using Godot;
using System;
using System.Collections.Generic;
using Gerbil.Utilities;

public class Map : Node2D
{
	private const string PathFindingPath = "/Pathfinding";
	private const string TileMapPath = "/TileMap";
	private const string SpikeTrapPath = "res://Traps/SpikeTrap.tscn";
	private const int TileSize = 32;
	private const int SpikeTrapID = 7;

	private Vector2 tileMargin = new Vector2(TileSize/2, TileSize/2);
	private TileMap tileMap;
	private PackedScene spikeTrapInstance;
	private Pathfinding pathfinding;

	public override void _Ready()
	{
		tileMap = GetNode<TileMap>(GetPath() + TileMapPath);
		spikeTrapInstance = ResourceLoader.Load<PackedScene>(SpikeTrapPath);
		SetTraps(GetTrapLocations());
		pathfinding = (Pathfinding)GetNode<Node2D>(GetPath() + PathFindingPath);
		pathfinding.CreateNavigationMap(tileMap);

		Debug.Instance.SetupGraphicOverly(this);
		Debug.Instance.DisplayPathFinding(true);
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
