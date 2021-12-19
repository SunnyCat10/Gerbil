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

	private Vector2[] TilesNearWall = new Vector2[4]{ 
		new Vector2(0,-1),
		new Vector2(1,0),
		new Vector2(0,1),
		new Vector2(-1,0)};

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
		AddConstantObstacles();
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

	/// <summary>
	/// Add constant obstacles to the tile map that will remain in the same state during the whole game.
	/// </summary>
	private void AddConstantObstacles()
	{
		Array obstacles = GetTree().GetNodesInGroup("Obstacles");
		foreach (var obstacleType in obstacles)
		{
			if (obstacleType is TileMap obstacleTypeTileMap)
			{
				Array tiles = obstacleTypeTileMap.GetUsedCells();
				foreach (Vector2 tile in tiles)
				{
					int id = CreateIdForPoint(tile);
					if (astar.HasPoint(id))
					{
						RemoveDiagonalsFromObstacle(tile); 
						astar.SetPointDisabled(id, true);
					}
				}
			}
		}	
	}

	/// <summary>
	/// Removes the diagonal paths from the tiles that are located at the four cardinal directions from the given tile.
	///	   [][X][]
	/// [X][Input][X]
	///	   [][X][]
	/// </summary>
	/// <param name="tile">Obstacle tile</param>
	private void RemoveDiagonalsFromObstacle(Vector2 tile)
	{
		int[] tilesNearbyIds = new int[4];
		for (int i = 0; i < tilesNearbyIds.Length; i++)
		{
			GD.Print((tile + TilesNearWall[i]).ToString());
			tilesNearbyIds[i] = CreateIdForPoint(tile + TilesNearWall[i]);
		}
		for (int i = 0; i < TilesNearWall.Length; i++) // Clear the paths from the tiles at the following indexes: [0][1]	[1][2]	[2][3]	[3][0]
		{
			if (i == 3) // Edge case where we taking the last and the first tile is the array.
			{
				if (astar.HasPoint(tilesNearbyIds[i]) && astar.HasPoint(tilesNearbyIds[0]))
					astar.DisconnectPoints(tilesNearbyIds[i], tilesNearbyIds[0]);
				break;
			}
			if (astar.HasPoint(tilesNearbyIds[i]) && astar.HasPoint(tilesNearbyIds[i+1]))
				astar.DisconnectPoints(tilesNearbyIds[i], tilesNearbyIds[i + 1]);
		}
	}
}
  
