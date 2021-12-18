using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class Pathfinding : Node2D
{
	private AStar2D astar;
	private TileMap tilemap;
	private Vector2 halfTileSize;
	private Rect2 tileMapBounds;

	private List<Vector2> debugPointsList;
	private List<Vector2> debugPathList;

	public override void _Ready()
	{
		astar = new AStar2D();
		debugPointsList = new List<Vector2>();
		debugPathList = new List<Vector2>();
	}

	public override void _Draw()
	{
		foreach (Vector2 point in debugPointsList)
		{
			DrawCircle(point, 3, Color.Color8(82, 62, 61));
		}
		for (int i = 0; i < debugPathList.Count - 1; i++)
		{	
			DrawLine(debugPathList[i], debugPathList[i + 1], Color.Color8(71, 121, 152), 2);
			DrawCircle(debugPathList[i], 3, Color.Color8(155, 197, 61));
			if (i == debugPathList.Count - 2)
				DrawCircle(debugPathList[debugPathList.Count - 1], 3, Color.Color8(155, 197, 61));
		}	
	}

	public void CreateNavigationMap(TileMap tilemap)
	{
		this.tilemap = tilemap;
		halfTileSize = tilemap.CellSize / 2;
		tileMapBounds = tilemap.GetUsedRect();

		Array tiles = tilemap.GetUsedCells();

		foreach (Vector2 point in tiles)
		{
			debugPointsList.Add(tilemap.MapToWorld(point) + halfTileSize);
		}
		Update();

		// Get array type
		AddTraversableTiles(tiles);
		ConnectTraversableTiles(tiles);
	}

	private void AddTraversableTiles(Array array)
	{
		foreach (Vector2 tile in array)
		{
			astar.AddPoint(CreateIdForPoint(tile), tile);
		}
	}

	private void ConnectTraversableTiles(Array array)
	{
		foreach (Vector2 tile in array)
		{
			int id = CreateIdForPoint(tile);

			for (int x = 0; x < 3; x++) // Get all the neighboring tiles including itself. 
			{
				for (int y = 0; y < 3; y++)
				{
					Vector2 target = tile + new Vector2(x - 1, y - 1);
					int targetId = CreateIdForPoint(target);

					if (tile == target || !astar.HasPoint(targetId))
						continue;
					astar.ConnectPoints(id, targetId, true);
				}
			}
		}
	}
	
	 
	// Create unique id for points-> like hash
	private int CreateIdForPoint(Vector2 point) 
	{
		float x = point.x - tileMapBounds.Position.x;
		float y = point.y - tileMapBounds.Position.y;

		return (int)(x + y * tileMapBounds.Size.x);
	}


	public Vector2[] GetNewPath(Vector2 start, Vector2 end)
	{
		int startId = CreateIdForPoint(tilemap.WorldToMap(start));
		int endId = CreateIdForPoint(tilemap.WorldToMap(end));

		if (!astar.HasPoint(startId) || !astar.HasPoint(endId))
			return new Vector2[0];

		Vector2[] pathInTileMapCoordinates = astar.GetPointPath(startId, endId);
		Vector2[] pathInWorldCoordinates = new Vector2[pathInTileMapCoordinates.Length];
		for (int i = 0; i < pathInTileMapCoordinates.Length; i++)
			pathInWorldCoordinates[i] = tilemap.MapToWorld(pathInTileMapCoordinates[i]) + halfTileSize; 
		return pathInWorldCoordinates;
	}

	public void DebugPath(List<Vector2> path)
	{
		debugPathList = path;
		Update();
	}
}
  
