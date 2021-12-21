using System.Collections.Generic;
using Godot;

namespace Gerbil.Utilities
{
    class DebugOverlay : Node2D
    {
        private const int PathfindingPointRadius = 3;
        private const int PathfindingLineWidth = 2;
        private readonly Color PathfindingPointColor = Color.Color8(155, 192, 61);
        private readonly Color PathfindingLineColor = Color.Color8(71, 121, 152);

        private bool isDisplayingPaths = false;
        
        private Dictionary<string, List<Vector2>> pathfindingPaths;

        public override void _Ready()
        {
            pathfindingPaths = new Dictionary<string, List<Vector2>>();
        }

        public override void _Process(float delta)
        {
            if (isDisplayingPaths)
                Update();
        }

        public override void _Draw()
        {
            foreach (List<Vector2> path in pathfindingPaths.Values)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    DrawLine(path[i], path[i + 1], PathfindingLineColor, PathfindingLineWidth);
                    DrawCircle(path[i], PathfindingPointRadius, PathfindingPointColor);
                    if (i == path.Count - 2)
                        DrawCircle(path[^1], PathfindingPointRadius, PathfindingPointColor);
                }
            }
        }

        /// <summary>
        /// Toggles the pathfinding display.
        /// </summary>
        /// <param name="display">display flag</param>
        public void DisplayPathfinding(bool display)
        {
            isDisplayingPaths = display;
        }
        
        /// <summary>
        /// Adds a pathfinding path to the displayed paths.
        /// </summary>
        /// <param name="entityName">Entity name for identification</param>
        /// <param name="pathfindingPath">Pathfinding path</param>
        public void AddPathfindingPath(string entityName, List<Vector2> pathfindingPath)
        {
            if (pathfindingPaths.ContainsKey(entityName))
                pathfindingPaths[entityName] = pathfindingPath;
            else
                pathfindingPaths.Add(entityName, pathfindingPath);
        }

        /// <summary>
        /// Removes a pathfinding path fromt he displayed paths.
        /// </summary>
        /// <param name="entityName">Entity name for identification</param>
        public void RemovePathfindingPath(string entityName)
        {
            if (pathfindingPaths.ContainsKey(entityName))
                pathfindingPaths.Remove(entityName);
        }
    }
}
